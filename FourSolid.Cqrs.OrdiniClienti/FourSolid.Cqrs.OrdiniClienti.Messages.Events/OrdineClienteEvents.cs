using FourSolid.Shared.InfoModel;
using FourSolid.Shared.Messages;
using FourSolid.Shared.ValueObjects;

namespace FourSolid.Cqrs.OrdiniClienti.Messages.Events
{
    public sealed class OrdineClienteCreated : EventBase
    {
        public readonly OrdineClienteId OrdineClienteId;
        public readonly ClienteId ClienteId;
        public readonly DataInserimento DataInserimento;
        public readonly DataPrevistaConsegna DataPrevistaConsegna;

        public OrdineClienteCreated(OrdineClienteId ordineClienteId, ClienteId clienteId,
            DataInserimento dataInserimento, DataPrevistaConsegna dataPrevistaConsegna, AccountInfo who,
            When when) : base(who, when)
        {
            this.SetAggregateIdFromDomainId(ordineClienteId);

            this.OrdineClienteId = ordineClienteId;
            this.ClienteId = clienteId;
            this.DataInserimento = dataInserimento;
            this.DataPrevistaConsegna = dataPrevistaConsegna;
        }
    }
}