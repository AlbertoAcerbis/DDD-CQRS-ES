using FourSolid.Cqrs.Anagrafiche.ReadModel.MongoDb.Abstracts;
using FourSolid.Cqrs.Anagrafiche.ReadModel.MongoDb.Documents;
using FourSolid.Cqrs.Anagrafiche.Shared.Configuration;
using Microsoft.Extensions.Options;

namespace FourSolid.Cqrs.Anagrafiche.ReadModel.MongoDb.Repository
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

        private IDocumentRepository<NoSqlCliente> _noSqlClienteRepository;
        public IDocumentRepository<NoSqlCliente> NoSqlClienteRepository =>
            this._noSqlClienteRepository ??
            (this._noSqlClienteRepository = new DocumentRepository<NoSqlCliente>(this._options));
    }
}