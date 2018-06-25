using FourSolid.Cqrs.Anagrafiche.ApplicationServices.Hubs;
using FourSolid.Cqrs.Anagrafiche.Messages.Events;
using FourSolid.Cqrs.Anagrafiche.Shared.ApplicationServices;
using Microsoft.AspNetCore.SignalR;
using Paramore.Brighter;

namespace FourSolid.Cqrs.Anagrafiche.ApplicationServices.Handlers.Clienti
{
    public class ClienteCreatedEventHandler : RequestHandler<ClienteCreated>
    {
        private readonly IClienteFactory _clienteFactory;
        private readonly IHubContext<ClientiHub> _clientiHubContext;

        public ClienteCreatedEventHandler(IClienteFactory clienteFactory, 
            IHubContext<ClientiHub> clientiHubContext)
        {
            this._clienteFactory = clienteFactory;
            this._clientiHubContext = clientiHubContext;
        }

        public override ClienteCreated Handle(ClienteCreated command)
        {
            this._clienteFactory
                .CreateClienteAsync(command.ClienteId, command.RagioneSociale, command.CodiceFiscale,
                    command.PartitaIva).GetAwaiter().GetResult();

            this._clientiHubContext.Clients.All.SendAsync("ClienteCreated", command.AggregateId.ToString());

            return base.Handle(command);
        }
    }
}