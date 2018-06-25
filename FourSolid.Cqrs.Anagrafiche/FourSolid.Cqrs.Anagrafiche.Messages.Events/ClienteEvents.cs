using FourSolid.Shared.InfoModel;
using FourSolid.Shared.Messages;
using FourSolid.Shared.ValueObjects;

namespace FourSolid.Cqrs.Anagrafiche.Messages.Events
{
    public sealed class ClienteCreated : EventBase
    {
        public readonly ClienteId ClienteId;
        public readonly RagioneSociale RagioneSociale;
        public readonly PartitaIva PartitaIva;
        public readonly CodiceFiscale CodiceFiscale;

        public ClienteCreated(ClienteId clienteId, RagioneSociale ragioneSociale, PartitaIva partitaIva,
            CodiceFiscale codiceFiscale, AccountInfo who, When when) : base(who, when)
        {
            this.SetAggregateIdFromDomainId(clienteId);

            this.ClienteId = clienteId;
            this.RagioneSociale = ragioneSociale;
            this.PartitaIva = partitaIva;
            this.CodiceFiscale = codiceFiscale;
        }
    }
}