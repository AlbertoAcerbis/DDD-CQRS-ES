using System;
using System.Threading;
using System.Threading.Tasks;
using Muflone.CommonDomain.Persistence;
using FourSolid.Cqrs.Anagrafiche.Domain.Factory;
using FourSolid.Cqrs.Anagrafiche.Messages.Commands;
using Paramore.Brighter;

namespace FourSolid.Cqrs.Anagrafiche.Domain.CommandsHandler.Clienti
{
    public class CreateClienteCommandHandler : RequestHandlerAsync<CreateCliente>
    {
        private readonly IRepository _repository;

        public CreateClienteCommandHandler(IRepository repository)
        {
            this._repository = repository;
        }

        public override async Task<CreateCliente> HandleAsync(CreateCliente command, CancellationToken cancellationToken = new CancellationToken())
        {
            var cliente = ClienteFactory.CreateCliente(command.ClienteId, command.RagioneSociale, command.CodiceFiscale,
                command.PartitaIva, command.Who, command.When);

            await this._repository.SaveAsync(cliente, Guid.NewGuid());

            return await base.HandleAsync(command, cancellationToken);
        }
    }
}