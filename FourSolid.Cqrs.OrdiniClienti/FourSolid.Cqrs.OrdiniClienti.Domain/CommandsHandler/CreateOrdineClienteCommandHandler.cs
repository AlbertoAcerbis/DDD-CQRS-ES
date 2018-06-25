using System;
using System.Threading;
using System.Threading.Tasks;
using Muflone.CommonDomain.Persistence;
using FourSolid.Cqrs.OrdiniClienti.Domain.Factory;
using FourSolid.Cqrs.OrdiniClienti.Messages.Commands;
using Paramore.Brighter;

namespace FourSolid.Cqrs.OrdiniClienti.Domain.CommandsHandler
{
    public class CreateOrdineClienteCommandHandler :
        RequestHandlerAsync<CreateOrdineCliente>
    {
        private readonly IRepository _repository;

        public CreateOrdineClienteCommandHandler(IRepository repository)
        {
            this._repository = repository;
        }

        public override async Task<CreateOrdineCliente> HandleAsync(CreateOrdineCliente command, CancellationToken cancellationToken = new CancellationToken())
        {
            var ordineCliente = OrdineClienteFactory.CreateOrdineClienteMaster(command.OrdineClienteId,
                command.ClienteId, command.DataInserimento, command.DataPrevistaConsegna, command.Who, command.When);

            await this._repository.SaveAsync(ordineCliente, Guid.NewGuid());

            return await base.HandleAsync(command, cancellationToken);
        }
    }
}