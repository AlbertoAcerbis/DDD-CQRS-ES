using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Muflone.CommonDomain;
using Muflone.CommonDomain.Core;
using Muflone.CommonDomain.Persistence;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FourSolid.EventStore
{
    public class EventStoreRepository : IRepository
    {
        private const string EventClrTypeHeader = "4SolidEvent";
        private const string AggregateClrTypeHeader = "4SolidAggregate";
        private const string CommitIdHeader = "CommitId";
        private const int WritePageSize = 500;
        private const int ReadPageSize = 500;

        private Func<Type, Guid, string> _aggregateIdToStreamName;

        private readonly IEventStoreConnection _eventStoreConnection;
        private static readonly JsonSerializerSettings SerializerSettings;

        static EventStoreRepository()
        {
            SerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None };
        }

        // Prima lettera minuscola ... per rimanere conmformi alla nomenclatura di js del GES
        public EventStoreRepository(IEventStoreConnection eventStoreConnection)
            : this(eventStoreConnection, (t, g) => $"{char.ToLower(t.Name[0]) + t.Name.Substring(1)}Events-{g}")
        {
        }

        public EventStoreRepository(IEventStoreConnection eventStoreConnection,
            Func<Type, Guid, string> aggregateIdToStreamName)
        {
            this._eventStoreConnection = eventStoreConnection;
            this._aggregateIdToStreamName = aggregateIdToStreamName;
        }

        public async Task<TAggregate> GetByIdAsync<TAggregate>(Guid id) where TAggregate : class, IAggregate
        {
            return await GetByIdAsync<TAggregate>(id, int.MaxValue);
        }

        public async Task<TAggregate> GetByIdAsync<TAggregate>(Guid id, int version) where TAggregate : class, IAggregate
        {
            if (version <= 0)
                throw new InvalidOperationException("Cannot get version <= 0");

            var streamName = this._aggregateIdToStreamName(typeof(TAggregate), id);
            var aggregate = ConstructAggregate<TAggregate>();

            var sliceStart = 0;
            StreamEventsSlice currentSlice;
            do
            {
                var sliceCount = sliceStart + ReadPageSize <= version ? ReadPageSize : version - sliceStart + 1;
                currentSlice =
                    await this._eventStoreConnection.ReadStreamEventsForwardAsync(streamName, sliceStart, sliceCount,
                        false);

                if (currentSlice.Status == SliceReadStatus.StreamNotFound)
                    throw new AggregateNotFoundException(id, typeof(TAggregate));

                if (currentSlice.Status == SliceReadStatus.StreamDeleted)
                    throw new AggregateDeletedException(id, typeof(TAggregate));

                sliceStart = (int)currentSlice.NextEventNumber;

                foreach (var evnt in currentSlice.Events)
                {
                    var eventToApply = DeserializeEvent(evnt.OriginalEvent.Metadata, evnt.OriginalEvent.Data);
                    if (eventToApply == null)
                        continue;
                    aggregate.ApplyEvent(eventToApply);
                }
            } while (version >= currentSlice.NextEventNumber && !currentSlice.IsEndOfStream);

            //if (aggregate.Version != version && version < int.MaxValue)
            //    throw new AggregateVersionException(id, typeof(TAggregate), aggregate.Version, version);

            return aggregate;
        }

        public async Task SaveAsync(IAggregate aggregate, Guid commitId)
        {
            await this.SaveAsync(aggregate, commitId, d => { });
        }

        public async Task SaveAsync(IAggregate aggregate, Guid commitId, Action<IDictionary<string, object>> updateHeaders)
        {
            var commitHeaders = new Dictionary<string, object>
            {
                { CommitIdHeader, commitId },
                { AggregateClrTypeHeader, aggregate.GetType().AssemblyQualifiedName }
            };
            updateHeaders(commitHeaders);

            var streamName = this._aggregateIdToStreamName(aggregate.GetType(), aggregate.Id);
            var newEvents = aggregate.GetUncommittedEvents().Cast<object>().ToList();
            var originalVersion = aggregate.Version - newEvents.Count;
            var expectedVersion = originalVersion == 0 ? ExpectedVersion.NoStream : originalVersion - 1;
            var eventsToSave = newEvents.Select(e => ToEventData(Guid.NewGuid(), e, commitHeaders)).ToList();

            if (eventsToSave.Count < WritePageSize)
            {
                await this._eventStoreConnection.AppendToStreamAsync(streamName, expectedVersion, eventsToSave);
            }
            else
            {
                var transaction = await this._eventStoreConnection.StartTransactionAsync(streamName, expectedVersion);

                var position = 0;
                while (position < eventsToSave.Count)
                {
                    var pageEvents = eventsToSave.Skip(position).Take(WritePageSize);
                    await transaction.WriteAsync(pageEvents);
                    position += WritePageSize;
                }

                await transaction.CommitAsync();
            }

            aggregate.ClearUncommittedEvents();
        }

        #region Helpers
        public static TAggregate ConstructAggregate<TAggregate>()
        {
            return (TAggregate)Activator.CreateInstance(typeof(TAggregate), true);
        }

        private static object DeserializeEvent(byte[] metadata, byte[] data)
        {
            try
            {
                var eventClrTypeName = JObject.Parse(Encoding.UTF8.GetString(metadata)).Property(EventClrTypeHeader).Value;
                return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), Type.GetType((string)eventClrTypeName), 
                    new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            }
            catch (Exception ex)
            {
                var dataEncoded = Encoding.UTF8.GetString(data);
                return null;
            }
        }

        private static EventData ToEventData(Guid eventId, object evnt, IDictionary<string, object> headers)
        {
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(evnt, SerializerSettings));

            var eventHeaders = new Dictionary<string, object>(headers)
            {
                {
                    EventClrTypeHeader, evnt.GetType().AssemblyQualifiedName
                }
            };
            var metadata = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventHeaders, SerializerSettings));
            var typeName = evnt.GetType().Name;

            return new EventData(eventId, typeName, true, data, metadata);
        }

        /// <summary>
        /// Replaced with constructor
        /// </summary>
        /// <param name="aggregate"></param>
        /// <param name="aggregateId"></param>
        private void StreamNameFactory(IAggregate aggregate, Guid aggregateId)
        {
            this._aggregateIdToStreamName =
                (t, g) =>
                    $"{char.ToLower(aggregate.GetType().Name[0])}{aggregate.GetType().Name.Substring(1)}-{aggregateId:N}";
        }
        #endregion

        #region Dispose
        private bool _disposed; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                this._disposed = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        //~EventStoreRepository()
        //{
        //    // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //    this.Dispose(false);
        //}

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            this.Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            //GC.SuppressFinalize(this);
        }
        #endregion
    }
}