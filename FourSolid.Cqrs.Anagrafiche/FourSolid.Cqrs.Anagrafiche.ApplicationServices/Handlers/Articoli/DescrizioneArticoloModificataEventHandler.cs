using FourSolid.Cqrs.Anagrafiche.Messages.Events;
using FourSolid.Cqrs.Anagrafiche.Shared.ApplicationServices;
using Paramore.Brighter;

namespace FourSolid.Cqrs.Anagrafiche.ApplicationServices.Handlers.Articoli
{
    public class DescrizioneArticoloModificataEventHandler :
        RequestHandler<DescrizioneArticoloModificata>
    {
        private readonly IArticoloFactory _articoloFactory;

        public DescrizioneArticoloModificataEventHandler(IArticoloFactory articoloFactory)
        {
            this._articoloFactory = articoloFactory;
        }

        public override DescrizioneArticoloModificata Handle(DescrizioneArticoloModificata command)
        {
            this._articoloFactory.ModificaDescrizioneArticoloAsync(command.ArticoloId, command.ArticoloDescrizione)
                .GetAwaiter().GetResult();

            return base.Handle(command);
        }
    }
}