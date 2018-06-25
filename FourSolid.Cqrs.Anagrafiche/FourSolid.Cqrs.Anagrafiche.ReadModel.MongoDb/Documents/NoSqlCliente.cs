using FourSolid.Cqrs.Anagrafiche.ReadModel.MongoDb.Abstracts;
using FourSolid.Cqrs.Anagrafiche.Shared.JsonModel;
using FourSolid.Shared.ValueObjects;

namespace FourSolid.Cqrs.Anagrafiche.ReadModel.MongoDb.Documents
{
    public class NoSqlCliente : DocumentEntity
    {
        public string RagioneSociale { get; private set; }
        public string PartitaIva { get; private set; }
        public string CodiceFiscale { get; private set; }

        protected NoSqlCliente()
        { }

        #region ctor
        public static NoSqlCliente CreateNoSqlCliente(ClienteId clienteId, RagioneSociale ragioneSociale,
            PartitaIva partitaIva, CodiceFiscale codiceFiscale)
        {
            return new NoSqlCliente(clienteId, ragioneSociale, partitaIva, codiceFiscale);
        }

        private NoSqlCliente(ClienteId clienteId, RagioneSociale ragioneSociale,
            PartitaIva partitaIva, CodiceFiscale codiceFiscale)
        {
            this.Id = clienteId.GetValue();
            this.RagioneSociale = ragioneSociale.GetValue();
            this.PartitaIva = partitaIva.GetValue();
            this.CodiceFiscale = codiceFiscale.GetValue();
        }
        #endregion

        #region ToJson
        public ClienteJson ToJson()
        {
            return new ClienteJson
            {
                ClienteId = this.Id,
                RagioneSociale = this.RagioneSociale,
                PartitaIva = this.PartitaIva,
                CodiceFiscale = this.CodiceFiscale
            };
        }
        #endregion
    }
}