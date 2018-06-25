using System.Threading.Tasks;
using FourSolid.Shared.ValueObjects;

namespace FourSolid.Cqrs.OrdiniClienti.Shared.ApplicationServices
{
    public interface IClienteFactory
    {
        Task CreateClienteAsync(ClienteId clienteId, RagioneSociale ragioneSociale);
    }
}