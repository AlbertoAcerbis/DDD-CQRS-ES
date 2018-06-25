using System.Collections.Generic;
using System.Threading.Tasks;
using FourSolid.Common.InProcessBus.Abstracts;
using FourSolid.Cqrs.Anagrafiche.Messages.Commands;
using FourSolid.Cqrs.Anagrafiche.Shared.ApplicationServices;
using FourSolid.Cqrs.Anagrafiche.Shared.JsonModel;
using FourSolid.Shared.InfoModel;
using FourSolid.Shared.ValueObjects;

namespace FourSolid.Cqrs.Anagrafiche.ApplicationServices.Orchestrator
{
    public class ClienteOrchestrator : IClienteOrchestrator
    {
        private readonly IServiceBus _serviceBus;
        private readonly IClienteFactory _clienteFactory;

        public ClienteOrchestrator(IServiceBus serviceBus, IClienteFactory clienteFactory)
        {
            this._serviceBus = serviceBus;
            this._clienteFactory = clienteFactory;
        }

        public async Task CreateClienteAsync(ClienteJson clienteToCreate, AccountInfo who, When when)
        {
            var createClienteCommand = new CreateCliente(new ClienteId(clienteToCreate.ClienteId),
                new RagioneSociale(clienteToCreate.RagioneSociale), new PartitaIva(clienteToCreate.PartitaIva),
                new CodiceFiscale(clienteToCreate.CodiceFiscale), who, when);

            await this._serviceBus.SendAsync(createClienteCommand);
        }

        public async Task<IEnumerable<ClienteJson>> GetClientiAsync() 
            => await this._clienteFactory.GetClientiAsync();

        public async Task<ClienteJson> GetClienteDetailsByIdAsync(string clienteId) =>
            await this._clienteFactory.GetClienteDetailsByIdAsync(new ClienteId(clienteId));
    }
}