using FourSolid.Cqrs.Anagrafiche.ReadModel.MongoDb.Documents;

namespace FourSolid.Cqrs.Anagrafiche.ReadModel.MongoDb.Abstracts
{
    public interface IDocumentUnitOfWork
    {
        IDocumentRepository<NoSqlArticolo> NoSqlArticoloRepository { get; }
        IDocumentRepository<NoSqlCliente> NoSqlClienteRepository { get; }
    }
}