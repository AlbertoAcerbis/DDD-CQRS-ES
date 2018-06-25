using System;
using System.Threading;
using System.Threading.Tasks;
using Muflone.CommonDomain.Persistence;
using FourSolid.Cqrs.Anagrafiche.Domain.Factory;
using FourSolid.Cqrs.Anagrafiche.Messages.Commands;
using Paramore.Brighter;

namespace FourSolid.Cqrs.Anagrafiche.Domain.CommandsHandler.Articoli
{
    public class CreateArticoloCommandHandler :
        RequestHandlerAsync<CreateArticolo>
    {
        private readonly IRepository _repository;

        public CreateArticoloCommandHandler(IRepository repository)
        {
            this._repository = repository;
        }

        public override async Task<CreateArticolo> HandleAsync(CreateArticolo command, CancellationToken cancellationToken = new CancellationToken())
        {
            var articolo = ArticoloFactory.CreateArticolo(command.ArticoloId, command.ArticoloDescrizione, command.UnitaMisura,
                command.ScortaMinima, command.Who, command.When);

            await this._repository.SaveAsync(articolo, Guid.NewGuid());

            return await base.HandleAsync(command, cancellationToken);
        }
    }
}