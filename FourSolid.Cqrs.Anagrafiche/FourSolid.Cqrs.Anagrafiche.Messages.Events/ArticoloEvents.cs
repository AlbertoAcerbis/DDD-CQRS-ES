using FourSolid.Shared.InfoModel;
using FourSolid.Shared.Messages;
using FourSolid.Shared.ValueObjects;

namespace FourSolid.Cqrs.Anagrafiche.Messages.Events
{
    public sealed class ArticoloCreated: EventBase
    {
        public readonly ArticoloId ArticoloId;
        public readonly ArticoloDescrizione ArticoloDescrizione;
        public readonly UnitaMisura UnitaMisura;
        public readonly ScortaMinima ScortaMinima;

        public ArticoloCreated(ArticoloId articoloId, ArticoloDescrizione articoloDescrizione, UnitaMisura unitaMisura,
            ScortaMinima scortaMinima, AccountInfo who, When when) : base(who, when)
        {
            this.SetAggregateIdFromDomainId(articoloId);

            this.ArticoloId = articoloId;
            this.ArticoloDescrizione = articoloDescrizione;
            this.UnitaMisura = unitaMisura;
            this.ScortaMinima = scortaMinima;
        }
    }
}
