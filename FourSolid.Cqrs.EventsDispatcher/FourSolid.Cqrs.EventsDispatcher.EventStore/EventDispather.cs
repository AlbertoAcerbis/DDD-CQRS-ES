using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using EventStore.ClientAPI;
using FourSolid.Cqrs.EventsDispatcher.EventStore.Abstract;
using FourSolid.Cqrs.EventsDispatcher.EventStore.Resources;
using FourSolid.Cqrs.EventsDispatcher.Logging.Abstracts;
using FourSolid.Cqrs.EventsDispatcher.MongoDb.Abstracts;
using FourSolid.Shared.InfoModel;
using FourSolid.Shared.JsonFolder;
using FourSolid.Shared.Services;
using FourSolid.Shared.ValueObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FourSolid.Cqrs.EventsDispatcher.EventStore
{
    public class EventDispatcher
    {
        public const string EventClrTypeHeader = "4SolidEvent";
        public const string AggregateClrTypeHeader = "4SolidAggregate";
        public const string CommitIdHeader = "CommitId";

        private const int ReconnectTimeoutMillisec = 5000;
        private const int ThreadKillTimeoutMillisec = 5000;
        private const int ReadPageSize = 500;
        private const int LiveQueueSizeLimit = 10000;

        private readonly IEventBus _eventBus;
        private readonly ILogService _logService;

        private readonly IEventStoreConnection _eventStoreConnection;
        private readonly ConcurrentQueue<ResolvedEvent> _liveQueue = new ConcurrentQueue<ResolvedEvent>();
        private readonly ManualResetEventSlim _liveDone = new ManualResetEventSlim(true);
        private readonly ConcurrentQueue<ResolvedEvent> _historicalQueue = new ConcurrentQueue<ResolvedEvent>();
        private readonly ManualResetEventSlim _historicalDone = new ManualResetEventSlim(true);
        private readonly IEventsFactory _eventsFactory;

        private volatile bool _stop;
        private volatile bool _livePublishingAllowed;
        private int _isPublishing;
        private Position _lastProcessed;
        private EventStoreSubscription _eventStoreSubscription;

        public EventDispatcher(IEventStoreConnection eventStoreConnection, IEventBus eventBus, ILogService logService, IEventsFactory eventsFactory)
        {
            this._eventStoreConnection = eventStoreConnection ?? throw new ArgumentNullException(nameof(eventStoreConnection));
            this._eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            this._logService = logService ?? throw new ArgumentNullException(nameof(logService));
            this._eventsFactory = eventsFactory;
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

            var eventPosition = this._eventsFactory.GetLastPositionAsync().GetAwaiter().GetResult();
            this._lastProcessed = new Position(eventPosition.CommitPosition, eventPosition.PreparePosition);

            Console.WriteLine(
                $"Start Reading EventStore from {this._lastProcessed.CommitPosition}/{this._lastProcessed.PreparePosition}");

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
            //TODO: Before trying to resubscribe - how to ensure that store is active and ready to accept.
            //AN: EventStoreConnection automatically tries to connect (if not already connected) to EventStore,
            //so you don't have to do something manually
            //Though in case of errors, you need to do some actions (if EventStore server is down or not yet up, etc)

            var task = this._eventStoreConnection.SubscribeToAllAsync(false, this.EventAppeared,
                this.SubscriptionDropped);
            if (!task.Wait(ReconnectTimeoutMillisec))
                throw new TimeoutException(EventStoreExceptions.ReconnectedAfterSubscriptionException);

            return task.Result;
        }

        private void SubscriptionDropped(EventStoreSubscription eventStoreSubscription, SubscriptionDropReason subscriptionDropReason, Exception arg3)
        {
            if (this._stop) return;

            this.RecoverSubscription();
        }

        private void EventAppeared(EventStoreSubscription eventStoreSubscription, ResolvedEvent resolvedEvent)
        {
            if (this._stop) return;

            this._liveQueue.Enqueue(resolvedEvent);

            //Prevent live queue memory explosion.
            if (!this._livePublishingAllowed && this._liveQueue.Count > LiveQueueSizeLimit)
            {
                ResolvedEvent throwAwayEvent;
                this._liveQueue.TryDequeue(out throwAwayEvent);
            }

            if (this._livePublishingAllowed)
                this.EnsurePublishEvents(this._liveQueue, this._liveDone);
        }

        private void EnsurePublishEvents(ConcurrentQueue<ResolvedEvent> queue, ManualResetEventSlim doneEvent)
        {
            if (this._stop) return;

            if (Interlocked.CompareExchange(ref this._isPublishing, 1, 0) == 0)
                ThreadPool.QueueUserWorkItem(_ => this.PublishEvents(queue, doneEvent));
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
                    if (!(evnt.OriginalPosition > this._lastProcessed))
                    {
                        if (evnt.OriginalPosition.HasValue)
                            Console.WriteLine($"Event Position: {evnt.OriginalPosition.Value.CommitPosition}; LastProcessed: {this._lastProcessed.CommitPosition}");
                        continue;
                    }

                    var processedEvent = ProcessRawEvent(evnt);
                    if (processedEvent != null)
                    {
                        this.SaveEventIntoReadModel(evnt);

                        var routingKey = GetRoutingKey(evnt);
                        this._logService.LoggerTrace($"Process event #{processedEvent.Id}");
                        Console.WriteLine($"Process event #{processedEvent.Id} / {routingKey}");
                        this._eventBus.PublishAsync(processedEvent, routingKey).Wait();
                    }
                    this._lastProcessed = evnt.OriginalPosition.Value;
                }

                doneEvent.Set(); // signal end of processing particular queue
                Interlocked.CompareExchange(ref this._isPublishing, 0, 1);

                // try to reacquire lock if needed
                keepGoing = !this._stop && queue.Count > 0 && Interlocked.CompareExchange(ref this._isPublishing, 1, 0) == 0;
            }
        }

        private void SaveEventIntoReadModel(ResolvedEvent rawEvent)
        {
            try
            {
                var position = rawEvent.OriginalPosition ?? new Position(0, 0);

                var eventId = rawEvent.Event.EventId;
                var eventType = rawEvent.Event.EventType;
                var eventStreamId = rawEvent.Event.EventStreamId;
                var eventStreamArray = eventStreamId.Split('-');
                var aggregateIdString = "";
                for (var i = 1; i <= 5; i++)
                    aggregateIdString = $"{aggregateIdString}{eventStreamArray[i]}";

                Guid.TryParse(aggregateIdString, out var aggregateId);
                var aggregateType = eventStreamArray[0].Replace("Events", "");
                var who = JObject.Parse(Encoding.UTF8.GetString(rawEvent.Event.Data)).Property("Who").Value;

                var accountInfo = JsonConvert.DeserializeObject<AccountInfo>(who.ToString());
                var eventDate = new When(rawEvent.Event.Created);

                this._eventsFactory.CreateEventsAsync(eventId, aggregateId, eventType, aggregateType, new EventStorePosition
                {
                    CommitPosition = position.CommitPosition,
                    PreparePosition = position.PreparePosition
                }, accountInfo, eventDate).GetAwaiter().GetResult();

                this._eventsFactory.SaveLastPositionAsync(new EventStorePosition
                {
                    CommitPosition = position.CommitPosition,
                    PreparePosition = position.PreparePosition
                }).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                this._logService.ErrorTrace("EventDispatcher.SaveEventIntoReadModel", ex);
            }
        }

        private static string GetRoutingKey(ResolvedEvent rawEvent)
        {
            try
            {
                var metadata = rawEvent.OriginalEvent.Metadata;
                var eventClrTypeName = JObject.Parse(Encoding.UTF8.GetString(metadata)).Property(EventClrTypeHeader).Value;
                var infoArray = eventClrTypeName.ToString().Split(',');
                var infoEvent = infoArray[0];
                var arrayEvent = infoEvent.Split('.');
                return arrayEvent[arrayEvent.Length - 1];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "UnresolvedEvent";
            }
        }

        private static dynamic ProcessRawEvent(ResolvedEvent rawEvent)
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
        private static dynamic DeserializeEvent(byte[] metadata, byte[] data)
        {
            if (JObject.Parse(Encoding.UTF8.GetString(metadata)).Property(EventClrTypeHeader) == null)
                return null;

            var eventClrTypeName = JObject.Parse(Encoding.UTF8.GetString(metadata)).Property(EventClrTypeHeader).Value;

            try
            {
                return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), Type.GetType((string)eventClrTypeName));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DeserializeEvent] - {CommonServices.GetErrorMessage(ex)}");
                return null;
            }
        }
    }
}