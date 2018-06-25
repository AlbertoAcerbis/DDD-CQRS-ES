using FourSolid.Shared.InfoModel;
using FourSolid.Shared.Messages;
using FourSolid.Shared.ValueObjects;

namespace FourSolid.Cqrs.OrdiniClienti.Messages.Events
{
    public sealed class ArticoloCreated : EventBase
    {
        public readonly ArticoloId ArticoloId;
        public readonly ArticoloDescrizione ArticoloDescrizione;

        public ArticoloCreated(ArticoloId articoloId, ArticoloDescrizione articoloDescrizione, AccountInfo who,
            When when) : base(who, when)
        {
            this.SetAggregateIdFromDomainId(articoloId);

            this.ArticoloId = articoloId;
            this.ArticoloDescrizione = articoloDescrizione;
        }
    }
}