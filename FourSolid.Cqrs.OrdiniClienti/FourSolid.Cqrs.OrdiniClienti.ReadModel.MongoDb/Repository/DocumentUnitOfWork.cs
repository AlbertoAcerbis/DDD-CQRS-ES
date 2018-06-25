using FourSolid.Cqrs.OrdiniClienti.ReadModel.MongoDb.Abstracts;
using FourSolid.Cqrs.OrdiniClienti.ReadModel.MongoDb.Documents;
using FourSolid.Cqrs.OrdiniClienti.Shared.Configuration;
using Microsoft.Extensions.Options;

namespace FourSolid.Cqrs.OrdiniClienti.ReadModel.MongoDb.Repository
{
    public class DocumentUnitOfWork : IDocumentUnitOfWork
    {
        private readonly IOptions<FourSettings> _options;

        #region ctor
        public DocumentUnitOfWork(IOptions<FourSettings> options)
        {
            this._options = options;
        }
        #endregion

        private IDocumentRepository<NoSqlArticolo> _noSqlArticoloRepository;
        public IDocumentRepository<NoSqlArticolo> NoSqlArticoloRepository =>
            this._noSqlArticoloRepository ??
            (this._noSqlArticoloRepository = new DocumentRepository<NoSqlArticolo>(this._options));

        private IDocumentRepository<NoSqlOrdineCliente> _noSqlOrdineClienteRepository;
        public IDocumentRepository<NoSqlOrdineCliente> NoSqlOrdineClienteRepository =>
            this._noSqlOrdineClienteRepository ?? (this._noSqlOrdineClienteRepository =
                new DocumentRepository<NoSqlOrdineCliente>(this._options));

        private IDocumentRepository<NoSqlCliente> _noSqlClienteRepository;
        public IDocumentRepository<NoSqlCliente> NoSqlClienteRepository =>
            this._noSqlClienteRepository ??
            (this._noSqlClienteRepository = new DocumentRepository<NoSqlCliente>(this._options));
    }
}