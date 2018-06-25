using System.Collections.Generic;
using FourSolid.Cqrs.OrdiniClienti.Domain.Abstracts;
using FourSolid.Cqrs.OrdiniClienti.Domain.Rules;
using FourSolid.Cqrs.OrdiniClienti.Messages.Events;
using FourSolid.Shared.InfoModel;
using FourSolid.Shared.ValueObjects;

namespace FourSolid.Cqrs.OrdiniClienti.Domain.Entities
{
    public class OrdineClienteMaster : AggregateRoot
    {
        private ClienteId _clienteId;
        private DataInserimento _dataInserimento;
        private DataPrevistaConsegna _dataPrevistaConsegna;

        private IEnumerable<OrdineClienteDetail> _ordineClienteDetails;

        protected OrdineClienteMaster()
        { }

        #region ctor
        internal OrdineClienteMaster(OrdineClienteId ordineClienteId, ClienteId clienteId,
            DataInserimento dataInserimento, DataPrevistaConsegna dataPrevistaConsegna, AccountInfo who, When when)
        {
            this.RaiseEvent(new OrdineClienteCreated(ordineClienteId, clienteId, dataInserimento, dataPrevistaConsegna,
                who, when));
        }

        private void Apply(OrdineClienteCreated @event)
        {
            this.Id = @event.AggregateId;

            this._clienteId = @event.ClienteId;
            this._dataInserimento = @event.DataInserimento;
            this._dataPrevistaConsegna = @event.DataPrevistaConsegna;
        }
        #endregion
    }
}