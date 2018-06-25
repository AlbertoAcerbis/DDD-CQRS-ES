using System;
using System.Collections.Generic;
using System.Linq;
using FourSolid.Cqrs.OrdiniClienti.ReadModel.MongoDb.Abstracts;
using FourSolid.Cqrs.OrdiniClienti.Shared.JsonModel;
using FourSolid.Shared.ValueObjects;

namespace FourSolid.Cqrs.OrdiniClienti.ReadModel.MongoDb.Documents
{
    public class NoSqlOrdineCliente : DocumentEntity
    {
        public string ClienteId { get; private set; }
        public string RagioneSociale { get; private set; }
        public DateTime DataInserimento { get; private set; }
        public DateTime DataPrevistaConsegna { get; private set; }

        public IEnumerable<OrdineDetailsJson> OrdineDetails { get; private set; }

        protected NoSqlOrdineCliente()
        { }

        #region ctor
        public static NoSqlOrdineCliente CreateNoSqlOrdineCliente(OrdineClienteId ordineClienteId,
            ClienteId clienteId, RagioneSociale ragioneSociale,
            DataInserimento dataInserimento, DataPrevistaConsegna dataPrevistaConsegna)
        {
            return new NoSqlOrdineCliente(ordineClienteId, clienteId, ragioneSociale, dataInserimento, dataPrevistaConsegna);
        }

        private NoSqlOrdineCliente(OrdineClienteId ordineClienteId, ClienteId clienteId, RagioneSociale ragioneSociale,
            DataInserimento dataInserimento, DataPrevistaConsegna dataPrevistaConsegna)
        {
            this.Id = ordineClienteId.GetValue();

            this.ClienteId = clienteId.GetValue();
            this.RagioneSociale = ragioneSociale.GetValue();
            this.DataInserimento = dataInserimento.GetValue();
            this.DataPrevistaConsegna = dataPrevistaConsegna.GetValue();

            this.OrdineDetails = Enumerable.Empty<OrdineDetailsJson>();
        }
        #endregion
    }
}