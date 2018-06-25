using FourSolid.Cqrs.EventsDispatcher.MongoDb.Abstracts;
using FourSolid.Cqrs.EventsDispatcher.MongoDb.Documents;

namespace FourSolid.Cqrs.EventsDispatcher.MongoDb.Repository
{
    public class DocumentUnitOfWork : IDocumentUnitOfWork
    {
        private IDocumentRepository<NoSqlEvents> _noSqlEventsRepository;
        public IDocumentRepository<NoSqlEvents> NoSqlEventsRepository =>
            this._noSqlEventsRepository ?? (this._noSqlEventsRepository = new DocumentRepository<NoSqlEvents>());

        private IDocumentRepository<NoSqlPosition> _noSqlPositionRepository;
        public IDocumentRepository<NoSqlPosition> NoSqlPositionRepository =>
            this._noSqlPositionRepository ?? (this._noSqlPositionRepository = new DocumentRepository<NoSqlPosition>());
    }
}