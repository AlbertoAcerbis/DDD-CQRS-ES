using FourSolid.Shared.InfoModel;
using FourSolid.Shared.Messages;
using FourSolid.Shared.ValueObjects;

namespace FourSolid.Cqrs.Anagrafiche.Messages.Commands
{
    public sealed class CreateArticolo : CommandBase
    {
        public readonly ArticoloId ArticoloId;
        public readonly ArticoloDescrizione ArticoloDescrizione;
        public readonly UnitaMisura UnitaMisura;
        public readonly ScortaMinima ScortaMinima;

        public CreateArticolo(ArticoloId articoloId, ArticoloDescrizione articoloDescrizione, UnitaMisura unitaMisura,
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
