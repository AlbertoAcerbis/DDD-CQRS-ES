using System.Collections.Generic;
using System.Threading.Tasks;
using FourSolid.Cqrs.Anagrafiche.Shared.JsonModel;
using FourSolid.Shared.InfoModel;
using FourSolid.Shared.ValueObjects;

namespace FourSolid.Cqrs.Anagrafiche.Shared.ApplicationServices
{
    public interface IArticoloOrchestrator
    {
        Task CreateArticoloAsync(ArticoloJson articoloToCreate, AccountInfo who, When when);

        Task<IEnumerable<ArticoloJson>> GetArticoliAsync();
        Task<ArticoloJson> GetArticoloByIdAsync(string articoloId);
    }
}