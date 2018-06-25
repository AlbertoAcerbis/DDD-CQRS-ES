using System;
using System.Linq;
using System.Threading.Tasks;
using FourSolid.Cqrs.EventsDispatcher.MongoDb.Abstracts;
using FourSolid.Cqrs.EventsDispatcher.MongoDb.Documents;
using FourSolid.Shared.InfoModel;
using FourSolid.Shared.JsonFolder;
using FourSolid.Shared.ValueObjects;
using MongoDB.Driver;

namespace FourSolid.Cqrs.EventsDispatcher.MongoDb.Services
{
    public class EventsFactory : IEventsFactory
    {
        private readonly IDocumentUnitOfWork _documentUnitOfWork;

        public EventsFactory(IDocumentUnitOfWork documentUnitOfWork)
        {
            this._documentUnitOfWork = documentUnitOfWork;
        }

        public async Task CreateEventsAsync(Guid eventGuid, Guid aggregateId, string eventType, string aggregateType,
            EventStorePosition position, AccountInfo who, When when)
        {
            try
            {
                var filter = Builders<NoSqlEvents>.Filter.Eq("_id", eventGuid.ToString());
                var documentsResult = await this._documentUnitOfWork.NoSqlEventsRepository.FindAsync(filter);

                if (documentsResult.Any())
                    return;

                var noSqlDocument = NoSqlEvents.CreateNoSqlEvents(eventGuid, aggregateId, eventType, aggregateType,
                    position, who, when);
                await this._documentUnitOfWork.NoSqlEventsRepository.InsertOneAsync(noSqlDocument);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task SaveLastPositionAsync(EventStorePosition position)
        {
            try
            {
                var filter = Builders<NoSqlPosition>.Filter.Empty;
                var documentResults = await this._documentUnitOfWork.NoSqlPositionRepository.FindAsync(filter);

                if (!documentResults.Any())
                {
                    var noSqlDocument = NoSqlPosition.CreateNoSqlPosition(position);
                    await this._documentUnitOfWork.NoSqlPositionRepository.InsertOneAsync(noSqlDocument);
                }
                else
                {
                    if (position.CommitPosition > documentResults.First().LastPosition.CommitPosition)
                    {
                        var update = documentResults.First().UpdateLastPostion(position);
                        filter = Builders<NoSqlPosition>.Filter.Eq("_id", documentResults.First().Id);
                        await this._documentUnitOfWork.NoSqlPositionRepository.UpdateOneAsync(filter, update);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<EventStorePosition> GetLastPositionAsync()
        {
            try
            {
                var filter = Builders<NoSqlPosition>.Filter.Empty;
                var documentResults = await this._documentUnitOfWork.NoSqlPositionRepository.FindAsync(filter);

                return documentResults.Any()
                    ? new EventStorePosition
                    {
                        CommitPosition = documentResults.First().LastPosition.CommitPosition,
                        PreparePosition = documentResults.First().LastPosition.PreparePosition
                    }
                    : new EventStorePosition {CommitPosition = -1, PreparePosition = -1};
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}