using System.Threading.Tasks;
using FourSolid.Cqrs.OrdiniClienti.Shared.JsonModel;
using FourSolid.Shared.InfoModel;
using FourSolid.Shared.ValueObjects;

namespace FourSolid.Cqrs.OrdiniClienti.Shared.ApplicationServices
{
    public interface IOrdineClienteOrchestrator
    {
        Task CreateOrdineClienteAsync(OrdineClienteJson ordineCliente, AccountInfo who, When when);
    }
}