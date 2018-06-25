using FourSolid.Cqrs.OrdiniClienti.ReadModel.MongoDb.Abstracts;
using FourSolid.Shared.ValueObjects;

namespace FourSolid.Cqrs.OrdiniClienti.ReadModel.MongoDb.Documents
{
    public class NoSqlCliente : DocumentEntity
    {
        public string RagioneSociale { get; private set; }

        protected NoSqlCliente()
        { }

        #region ctor
        public static NoSqlCliente CreateNoSqlCliente(ClienteId clienteId, RagioneSociale ragioneSociale)
        {
            return new NoSqlCliente(clienteId, ragioneSociale);
        }

        private NoSqlCliente(ClienteId clienteId, RagioneSociale ragioneSociale)
        {
            this.Id = clienteId.GetValue();
            this.RagioneSociale = ragioneSociale.GetValue();
        }
        #endregion
    }
}