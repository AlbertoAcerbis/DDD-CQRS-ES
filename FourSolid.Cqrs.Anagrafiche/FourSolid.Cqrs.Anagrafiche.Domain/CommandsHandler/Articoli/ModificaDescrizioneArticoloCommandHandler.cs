using System;
using System.Threading;
using System.Threading.Tasks;
using FourSolid.Cqrs.Anagrafiche.Domain.Entities;
using FourSolid.Cqrs.Anagrafiche.Messages.Commands;
using Muflone.CommonDomain.Persistence;
using Paramore.Brighter;

namespace FourSolid.Cqrs.Anagrafiche.Domain.CommandsHandler.Articoli
{
    public class ModificaDescrizioneArticoloCommandHandler : RequestHandlerAsync<ModificaDescrizioneArticolo>
    {
        private readonly IRepository _repository;

        public ModificaDescrizioneArticoloCommandHandler(IRepository repository)
        {
            this._repository = repository;
        }

        public override async Task<ModificaDescrizioneArticolo> HandleAsync(ModificaDescrizioneArticolo command, CancellationToken cancellationToken = new CancellationToken())
        {
            var articolo = await this._repository.GetByIdAsync<Articolo>(command.AggregateId);

            articolo.ModificaDescrizioneArticolo(command.ArticoloId, command.ArticoloDescrizione, command.Who,
                command.When);

            await this._repository.SaveAsync(articolo, Guid.NewGuid());

            return await base.HandleAsync(command, cancellationToken);
        }
    }
}