using FourSolid.Cqrs.Anagrafiche.Domain.Abstracts;
using FourSolid.Cqrs.Anagrafiche.Messages.Events;
using FourSolid.Shared.InfoModel;
using FourSolid.Shared.ValueObjects;

namespace FourSolid.Cqrs.Anagrafiche.Domain.Entities
{
    public class Cliente : AggregateRoot
    {
        private RagioneSociale _ragioneSociale;
        private CodiceFiscale _codiceFiscale;
        private PartitaIva _partitaIva;

        protected Cliente()
        { }

        #region ctor
        internal Cliente(ClienteId clienteId, RagioneSociale ragioneSociale,
            CodiceFiscale codiceFiscale, PartitaIva partitaIva, AccountInfo who, When when)
        {
            this.RaiseEvent(new ClienteCreated(clienteId, ragioneSociale, partitaIva, codiceFiscale, who, when));
        }

        private void Apply(ClienteCreated @event)
        {
            this.Id = @event.AggregateId;

            this._ragioneSociale = @event.RagioneSociale;
            this._partitaIva = @event.PartitaIva;
            this._codiceFiscale = @event.CodiceFiscale;
        }
        #endregion
    }
}