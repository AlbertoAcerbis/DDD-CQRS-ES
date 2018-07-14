using FourSolid.Cqrs.Anagrafiche.Domain.Entities;
using FourSolid.Cqrs.Anagrafiche.Domain.Rules;
using FourSolid.Shared.InfoModel;
using FourSolid.Shared.ValueObjects;

namespace FourSolid.Cqrs.Anagrafiche.Domain.Factory
{
    internal class ArticoloFactory
    {
        internal static Articolo CreateArticolo(ArticoloId articoloId, ArticoloDescrizione articoloDescrizione, UnitaMisura unitaMisura,
            ScortaMinima scortaMinima, AccountInfo who, When when)
        {
            DomainRules.ChkArticoloId(articoloId);
            DomainRules.ChkArticoloDescrizione(articoloDescrizione);
            DomainRules.ChkUnitaMisura(unitaMisura);

            return new Articolo(articoloId, articoloDescrizione, unitaMisura, scortaMinima, who, when);
        }
    }
}