using FourSolid.Cqrs.Anagrafiche.Domain.Abstracts;
using FourSolid.Cqrs.Anagrafiche.Domain.Rules;
using FourSolid.Cqrs.Anagrafiche.Messages.Events;
using FourSolid.Shared.InfoModel;
using FourSolid.Shared.ValueObjects;

namespace FourSolid.Cqrs.Anagrafiche.Domain.Entities
{
    public class Articolo : AggregateRoot
    {
        private ArticoloDescrizione _articoloDescrizione;
        private UnitaMisura _unitaMisura;
        private ScortaMinima _scortaMinima;

        protected Articolo()
        { }

        #region ctor
        internal Articolo(ArticoloId articoloId, ArticoloDescrizione articoloDescrizione, UnitaMisura unitaMisura,
            ScortaMinima scortaMinima, AccountInfo who, When when)
        {
            this.RaiseEvent(new ArticoloCreated(articoloId, articoloDescrizione, unitaMisura, scortaMinima, who, when));
        }

        private void Apply(ArticoloCreated @event)
        {
            this.Id = @event.AggregateId;

            this._articoloDescrizione = @event.ArticoloDescrizione;
            this._unitaMisura = @event.UnitaMisura;
            this._scortaMinima = @event.ScortaMinima;
        }
        #endregion

        internal void ModificaDescrizioneArticolo(ArticoloId articoloId, ArticoloDescrizione descrizione,
            AccountInfo who, When when)
        {
            DomainRules.ChkArticoloDescrizione(descrizione);

            this.RaiseEvent(new DescrizioneArticoloModificata(articoloId, descrizione, who, when));
        }

        private void Apply(DescrizioneArticoloModificata @event)
        {
            this._articoloDescrizione = @event.ArticoloDescrizione;
        }
    }
}