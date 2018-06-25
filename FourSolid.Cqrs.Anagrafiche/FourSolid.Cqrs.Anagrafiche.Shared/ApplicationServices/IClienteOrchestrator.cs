using System.Collections.Generic;
using System.Threading.Tasks;
using FourSolid.Cqrs.Anagrafiche.Shared.JsonModel;
using FourSolid.Shared.InfoModel;
using FourSolid.Shared.ValueObjects;

namespace FourSolid.Cqrs.Anagrafiche.Shared.ApplicationServices
{
    public interface IClienteOrchestrator
    {
        Task CreateClienteAsync(ClienteJson clienteToCreate, AccountInfo who, When when);

        Task<IEnumerable<ClienteJson>> GetClientiAsync();
        Task<ClienteJson> GetClienteDetailsByIdAsync(string clienteId);
    }
}