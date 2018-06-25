using FourSolid.Cqrs.EventsDispatcher.MongoDb.Documents;

namespace FourSolid.Cqrs.EventsDispatcher.MongoDb.Abstracts
{
    public interface IDocumentUnitOfWork
    {
        IDocumentRepository<NoSqlEvents> NoSqlEventsRepository { get; }
        IDocumentRepository<NoSqlPosition> NoSqlPositionRepository { get; }
    }
}