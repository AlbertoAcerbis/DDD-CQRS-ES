using FourSolid.Shared.InfoModel;
using FourSolid.Shared.Messages;
using FourSolid.Shared.ValueObjects;

namespace FourSolid.Cqrs.OrdiniClienti.Messages.Commands
{
    public sealed class CreateOrdineCliente : CommandBase
    {
        public readonly OrdineClienteId OrdineClienteId;
        public readonly ClienteId ClienteId;
        public readonly DataInserimento DataInserimento;
        public readonly DataPrevistaConsegna DataPrevistaConsegna;

        public CreateOrdineCliente(OrdineClienteId ordineClienteId, ClienteId clienteId,
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