using FourSolid.Cqrs.OrdiniClienti.ReadModel.MongoDb.Documents;

namespace FourSolid.Cqrs.OrdiniClienti.ReadModel.MongoDb.Abstracts
{
    public interface IDocumentUnitOfWork
    {
        IDocumentRepository<NoSqlArticolo> NoSqlArticoloRepository { get; }

        IDocumentRepository<NoSqlOrdineCliente> NoSqlOrdineClienteRepository { get; }

        IDocumentRepository<NoSqlCliente> NoSqlClienteRepository { get; }
    }
}