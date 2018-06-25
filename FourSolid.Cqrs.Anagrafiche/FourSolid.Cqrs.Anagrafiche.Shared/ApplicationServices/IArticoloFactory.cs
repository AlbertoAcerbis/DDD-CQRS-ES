using System.Collections.Generic;
using System.Threading.Tasks;
using FourSolid.Cqrs.Anagrafiche.Shared.JsonModel;
using FourSolid.Shared.ValueObjects;

namespace FourSolid.Cqrs.Anagrafiche.Shared.ApplicationServices
{
    public interface IArticoloFactory
    {
        Task CreateArticoloAsync(ArticoloId articoloId, ArticoloDescrizione articoloDescrizione,
            UnitaMisura unitaMisura, ScortaMinima scortaMinima);

        Task<IEnumerable<ArticoloJson>> GetArticoliAsync();
        Task<ArticoloJson> GetArticoloByIdAsync(ArticoloId articoloId);
    }
}