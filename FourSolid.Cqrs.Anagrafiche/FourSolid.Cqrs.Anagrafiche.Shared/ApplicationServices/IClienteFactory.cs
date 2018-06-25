using System.Collections.Generic;
using System.Threading.Tasks;
using FourSolid.Cqrs.Anagrafiche.Shared.JsonModel;
using FourSolid.Shared.ValueObjects;

namespace FourSolid.Cqrs.Anagrafiche.Shared.ApplicationServices
{
    public interface IClienteFactory
    {
        Task CreateClienteAsync(ClienteId clienteId, RagioneSociale ragioneSociale, CodiceFiscale codiceFiscale,
            PartitaIva partitaIva);

        Task<IEnumerable<ClienteJson>> GetClientiAsync();
        Task<ClienteJson> GetClienteDetailsByIdAsync(ClienteId clienteId);
    }
}