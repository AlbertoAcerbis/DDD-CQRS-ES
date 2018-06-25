using System;
using System.Threading.Tasks;
using FourSolid.Shared.InfoModel;
using FourSolid.Shared.JsonFolder;
using FourSolid.Shared.ValueObjects;

namespace FourSolid.Cqrs.EventsDispatcher.MongoDb.Abstracts
{
    public interface IEventsFactory
    {
        Task CreateEventsAsync(Guid eventGuid, Guid aggregateId, string eventType, string aggregateType,
            EventStorePosition position, AccountInfo who, When when);

        Task SaveLastPositionAsync(EventStorePosition position);
        Task<EventStorePosition> GetLastPositionAsync();
    }
}