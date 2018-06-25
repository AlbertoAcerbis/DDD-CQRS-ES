using System;
using FourSolid.Cqrs.EventsDispatcher.MongoDb.Abstracts;
using FourSolid.Shared.InfoModel;
using FourSolid.Shared.JsonFolder;
using FourSolid.Shared.ValueObjects;

namespace FourSolid.Cqrs.EventsDispatcher.MongoDb.Documents
{
    public class NoSqlEvents : DocumentEntity
    {
        public string AggregateId { get; private set; }
        public string AggregateType { get; private set; }
        public string EventType { get; private set; }
        public AccountJson Who { get; private set; }
        public DateTime When { get; private set; }
        public EventStorePosition Position { get; private set; }

        protected NoSqlEvents()
        { }

        #region ctor
        public static NoSqlEvents CreateNoSqlEvents(Guid eventGuid, Guid aggregateId, string eventType,
            string aggregateType, EventStorePosition position, AccountInfo who, When when)
        {
            return new NoSqlEvents(eventGuid, aggregateId, eventType, aggregateType, position, who, when);
        }

        private NoSqlEvents(Guid eventGuid, Guid aggregateId, string eventType,
            string aggregateType, EventStorePosition position, AccountInfo who, When when)
        {
            this.Id = eventGuid.ToString();
            this.AggregateId = aggregateId.ToString("N");
            this.AggregateType = aggregateType;
            this.EventType = eventType;
            this.Who = who.ToJson();
            this.When = when.GetValue();
            this.Position = position;
        }
        #endregion
    }
}