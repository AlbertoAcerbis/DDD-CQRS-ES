using FourSolid.Cqrs.OrdiniClienti.Messages.Events;
using FourSolid.Cqrs.OrdiniClienti.Shared.ApplicationServices;
using Paramore.Brighter;

namespace FourSolid.Cqrs.OrdiniClienti.ApplicationServices.Handlers.Clienti
{
    public class ClienteCreatedEventHandler : RequestHandler<ClienteCreated>
    {
        private readonly IClienteFactory _clienteFactory;

        public ClienteCreatedEventHandler(IClienteFactory clienteFactory)
        {
            this._clienteFactory = clienteFactory;
        }

        public override ClienteCreated Handle(ClienteCreated command)
        {
            this._clienteFactory.CreateClienteAsync(command.ClienteId, command.RagioneSociale).GetAwaiter().GetResult();

            return base.Handle(command);
        }
    }
}