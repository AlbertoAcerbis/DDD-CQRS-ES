using System.Threading.Tasks;
using FourSolid.Shared.ValueObjects;

namespace FourSolid.Cqrs.OrdiniClienti.Shared.ApplicationServices
{
    public interface IArticoloFactory
    {
        Task CreateArticoloAsync(ArticoloId articoloId, ArticoloDescrizione articoloDescrizione);
        Task ModificaDescrizioneArticoloAsync(ArticoloId articoloId, ArticoloDescrizione descrizione);
    }
}