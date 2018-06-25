using System;
using FourSolid.Cqrs.EventsDispatcher.MongoDb.Abstracts;
using FourSolid.Shared.JsonFolder;
using MongoDB.Driver;

namespace FourSolid.Cqrs.EventsDispatcher.MongoDb.Documents
{
    public class NoSqlPosition : DocumentEntity
    {
        public EventStorePosition LastPosition { get; private set; }

        protected NoSqlPosition()
        { }

        public static NoSqlPosition CreateNoSqlPosition(EventStorePosition eventStorePosition)
        {
            return new NoSqlPosition(eventStorePosition);
        }

        private NoSqlPosition(EventStorePosition eventStorePosition)
        {
            this.LastPosition = eventStorePosition;
            this.Id = Guid.NewGuid().ToString();
        }

        public UpdateDefinition<NoSqlPosition> UpdateLastPostion(EventStorePosition eventStorePosition)
        {
            return Builders<NoSqlPosition>.Update.Set(p => p.LastPosition, eventStorePosition);
        }
    }
}