using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using FourSolid.Common.InProcessBus.Abstracts;
using FourSolid.EventStore.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Paramore.Brighter;

namespace FourSolid.EventStore
{
    public class EventDispatcher
    {
        private const int ReconnectTimeoutMillisec = 5000;
        private const int ThreadKillTimeoutMillisec = 5000;
        private const int ReadPageSize = 500;
        public const int WritePageSize = 500;
        private const int LiveQueueSizeLimit = 10000;

        public const string EventClrTypeHeader = "4SolidEvent";             // EventClrTypeName
        public const string AggregateClrTypeHeader = "4SolidAggregate";     // AggregateClrTypeName
        public const string CommitIdHeader = "CommitId";

        private readonly IEventBus _eventBus;

        private readonly IEventStoreConnection _eventStoreConnection;
        private readonly ConcurrentQueue<ResolvedEvent> _liveQueue = new ConcurrentQueue<ResolvedEvent>();
        private readonly ManualResetEventSlim _liveDone = new ManualResetEventSlim(true);
        private readonly ConcurrentQueue<ResolvedEvent> _historicalQueue = new ConcurrentQueue<ResolvedEvent>();
        private readonly ManualResetEventSlim _historicalDone = new ManualResetEventSlim(true);

        private static readonly ResolvedEvent DropSubscriptionEvent = new ResolvedEvent();
        private readonly Action<EventStoreCatchUpSubscription, SubscriptionDropReason, Exception> _subscriptionDropped;

        private volatile bool _stop;
        private volatile bool _livePublishingAllowed;
        private int _isPublishing;
        private Position _lastProcessed;
        private EventStoreSubscription _eventStoreSubscription;
        //private readonly ILogger _logger;

        private DropData _dropData;
        private int _isProcessing;
        private int _isDropped;
        private volatile bool _allowProcessing;

        public EventDispatcher(IEventStoreConnection eventStoreConnection, IEventBus eventBus)
        {
            this._eventStoreConnection = eventStoreConnection ?? throw new ArgumentNullException(nameof(eventStoreConnection));
            this._eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            this._lastProcessed = new Position(-1, -1);
        }

        // Credit algorithm to Szymon Pobiega
        // http://simon-says-architecture.com/2013/02/02/mechanics-of-durable-subscription/#comments
        // 1. The subscriber always starts with pull assuming there were some messages generated while it was offline
        // 2. The subscriber pulls messages until thereâ€™s nothing left to pull (it is up to date with the stream)
        // 3. Push subscription is started  but arriving messages are not processed immediately but temporarily redirected to a buffer
        // 4. One last pull is done to ensure nothing happened between step 2 and 3
        // 5. Messages from this last pull are processed
        // 6. Processing messages from push buffer is started. While messages are processed, they are checked against IDs of messages processed in step 5 to ensure thereâ€™s no duplicates.
        // 7. System works in push model until subscriber is killed or subscription is dropped by publisher drops push subscription.

        // Credit to Andrii Nakryiko
        // If data is written to storage at such a speed, that between the moment you did your last 
        // pull read and the moment you subscribed to push notifications more data (events) were 
        // generated, than you request in one pull request, you would need to repeat steps 4-5 few 
        // times until you get a pull message which position is >= subscription position 
        // (EventStore provides you with those positions).
        public void StartDispatching()
        {
            this.RecoverSubscription();
        }

        private void RecoverSubscription()
        {
            this._livePublishingAllowed = false;
            this._liveDone.Wait(); // wait until all live processing is finished (queue is empty, _lastProcessed updated)
            this._allowProcessing = true;

            //AN: if _lastProcessed == (-1, -1) then we haven't processed anything yet, so we start from Position.Start
            var startPos = this._lastProcessed == new Position(-1, -1) ? Position.Start : this._lastProcessed;
            var nextPos = this.ReadHistoricalEventsFrom(startPos);

            this._eventStoreSubscription = this.SubscribeToAll();

            this.ReadHistoricalEventsFrom(nextPos);
            this._historicalDone.Wait(); // wait until historical queue is empty and _lastProcessed updated

            this._livePublishingAllowed = true;
            this.EnsurePublishEvents(this._liveQueue, this._liveDone);
        }

        public void StopDispatching()
        {
            this._stop = true;
            this._eventStoreSubscription?.Unsubscribe();

            // hopefully additional check in PublishEvents (additional check for _stop after setting event) prevents race conditions
            if (!this._historicalDone.Wait(ThreadKillTimeoutMillisec))
                throw new TimeoutException(EventStoreExceptions.DispatchStoppingException);

            if (!this._liveDone.Wait(ThreadKillTimeoutMillisec))
                throw new TimeoutException(EventStoreExceptions.DispatchStoppingException);
        }

        private Position ReadHistoricalEventsFrom(Position from)
        {
            var position = from;

            AllEventsSlice allEventsSlice;
            while (!this._stop && (allEventsSlice = this._eventStoreConnection.ReadAllEventsForwardAsync(position, ReadPageSize, false).Result).Events.Length > 0)
            {
                foreach (var rawEvent in allEventsSlice.Events)
                {
                    this._historicalQueue.Enqueue(rawEvent);
                }
                this.EnsurePublishEvents(this._historicalQueue, this._historicalDone);

                position = allEventsSlice.NextPosition;
            }

            return position;
        }

        private EventStoreSubscription SubscribeToAll()
        {
            //EventStoreAllCatchUpSubscription catchUpAll = new EventStoreAllCatchUpSubscription(this._eventStoreConnection, this._logger, this._lastProcessed, new UserCredentials("admin", "changeit"),  

            //TODO: Before trying to resubscribe - how to ensure that store is active and ready to accept.
            //AN: EventStoreConnection automatically tries to connect (if not already connected) to EventStore,
            //so you don't have to do something manually
            //Though in case of errors, you need to do some actions (if EventStore server is down or not yet up, etc)

            var task = this._eventStoreConnection.SubscribeToAllAsync(false, this.EnqueuePushedEvent, this.SubscriptionDropped);
            if (!task.Wait(ReconnectTimeoutMillisec))
                throw new TimeoutException(EventStoreExceptions.ReconnectedAfterSubscriptionException);

            return task.Result;
        }

        private Task EnqueuePushedEvent(EventStoreSubscription subscription, ResolvedEvent e)
        {
            if (this._liveQueue.Count >= LiveQueueSizeLimit)
            {
                EnqueueSubscriptionDropNotification(SubscriptionDropReason.ProcessingQueueOverflow, null);
                subscription.Unsubscribe();
                return Task.CompletedTask;
            }

            this._liveQueue.Enqueue(e);

            if (this._livePublishingAllowed)
                EnsureProcessingPushQueue();

            return Task.CompletedTask;
        }

        private void EnqueueSubscriptionDropNotification(SubscriptionDropReason reason, Exception error)
        {
            // if drop data was already set -- no need to enqueue drop again, somebody did that already
            var dropData = new DropData(reason, error);
            if (Interlocked.CompareExchange(ref this._dropData, dropData, null) == null)
            {
                this._liveQueue.Enqueue(DropSubscriptionEvent);
                if (this._allowProcessing)
                    this.EnsureProcessingPushQueue();
            }
        }

        private void SubscriptionDropped(EventStoreSubscription eventStoreSubscription, SubscriptionDropReason subscriptionDropReason, Exception arg3)
        {
            if (this._stop) return;

            this.RecoverSubscription();
        }

        private void EnsurePublishEvents(ConcurrentQueue<ResolvedEvent> queue, ManualResetEventSlim doneEvent)
        {
            if (this._stop) return;

            if (Interlocked.CompareExchange(ref this._isPublishing, 1, 0) == 0)
                ThreadPool.QueueUserWorkItem(_ => this.PublishEvents(queue, doneEvent));
        }
        private void EnsureProcessingPushQueue()
        {
            if (Interlocked.CompareExchange(ref this._isProcessing, 1, 0) == 0)
                ThreadPool.QueueUserWorkItem(_ => ProcessLiveQueueAsync());
        }

        private async void ProcessLiveQueueAsync()
        {
            do
            {
                while (this._liveQueue.TryDequeue(out var e))
                {
                    if (e.Equals(DropSubscriptionEvent)) // drop subscription artificial ResolvedEvent
                    {
                        this._dropData = this._dropData ?? new DropData(SubscriptionDropReason.Unknown, new Exception("Drop reason not specified."));
                        this.DropSubscription(this._dropData.Reason, this._dropData.Error);
                        Interlocked.CompareExchange(ref this._isProcessing, 0, 1);
                        return;
                    }

                    try
                    {
                        await TryProcessAsync(e).ConfigureAwait(false);
                    }
                    catch (Exception exc)
                    {
                        //Log.Debug("Catch-up Subscription {0} to {1} Exception occurred in subscription {1}", SubscriptionName, IsSubscribedToAll ? "<all>" : StreamId, exc);
                        DropSubscription(SubscriptionDropReason.EventHandlerException, exc);
                        return;
                    }
                }
                Interlocked.CompareExchange(ref this._isProcessing, 0, 1);
            } while (this._liveQueue.Count > 0 && Interlocked.CompareExchange(ref this._isProcessing, 1, 0) == 0);
        }

        internal void DropSubscription(SubscriptionDropReason reason, Exception error)
        {
            if (Interlocked.CompareExchange(ref this._isDropped, 1, 0) == 0)
            {
                this._eventStoreSubscription?.Unsubscribe();

                //this._subscriptionDropped?.Invoke(this, reason, error);
                this._historicalDone.Set();
            }
        }

        /// <summary>
        /// Try to process a single <see cref="ResolvedEvent"/>.
        /// </summary>
        /// <param name="e">The <see cref="ResolvedEvent"/> to process.</param>
        protected async Task TryProcessAsync(ResolvedEvent e)
        {
            bool processed = false;
            if (e.OriginalPosition > this._lastProcessed)
            {
                try
                {
                    await this.EnqueuePushedEvent(this._eventStoreSubscription, e).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    DropSubscription(SubscriptionDropReason.EventHandlerException, ex);
                    throw;
                }
                //this._lastProcessed = e.OriginalPosition.Value;
                processed = true;
            }
        }

        private class DropData
        {
            public readonly SubscriptionDropReason Reason;
            public readonly Exception Error;

            public DropData(SubscriptionDropReason reason, Exception error)
            {
                this.Reason = reason;
                this.Error = error;
            }
        }

        private void PublishEvents(ConcurrentQueue<ResolvedEvent> queue, ManualResetEventSlim doneEvent)
        {
            var keepGoing = true;
            while (keepGoing)
            {
                doneEvent.Reset(); // signal we start processing this queue
                if (this._stop) // this is to avoid race condition in StopDispatching, though it is 1AM here, so I could be wrong :)
                {
                    doneEvent.Set();
                    Interlocked.CompareExchange(ref this._isPublishing, 0, 1);
                    return;
                }

                while (!this._stop && queue.TryDequeue(out var evnt))
                {
                    if (!(evnt.OriginalPosition > this._lastProcessed)) continue;

                    var processedEvent = ProcessRawEvent(evnt);
                    if (processedEvent != null)
                        this._eventBus.PublishAsync(processedEvent);
                    this._lastProcessed = evnt.OriginalPosition.Value;
                }

                doneEvent.Set(); // signal end of processing particular queue
                Interlocked.CompareExchange(ref this._isPublishing, 0, 1);

                // try to reacquire lock if needed
                keepGoing = !this._stop && queue.Count > 0 && Interlocked.CompareExchange(ref this._isPublishing, 1, 0) == 0;
            }
        }

        private static Event ProcessRawEvent(ResolvedEvent rawEvent)
        {
            if (rawEvent.OriginalEvent.Metadata.Length > 0 && rawEvent.OriginalEvent.Data.Length > 0)
                return DeserializeEvent(rawEvent.OriginalEvent.Metadata, rawEvent.OriginalEvent.Data);

            return null;
        }

        /// <summary>
        /// Deserializes the event from the raw GetEventStore event to my event.
        /// Took this from a gist that James Nugent posted on the GetEventStore forums.
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static Event DeserializeEvent(byte[] metadata, byte[] data)
        {
            if (JObject.Parse(Encoding.UTF8.GetString(metadata)).Property(EventClrTypeHeader) == null)
                return null;

            var eventClrTypeName = JObject.Parse(Encoding.UTF8.GetString(metadata)).Property(EventClrTypeHeader).Value;

            try
            {
                return (Event)JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), Type.GetType((string)eventClrTypeName));
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}