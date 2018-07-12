using FourSolid.Cqrs.OrdiniClienti.Messages.Events;
using FourSolid.Cqrs.OrdiniClienti.Shared.ApplicationServices;
using Paramore.Brighter;

namespace FourSolid.Cqrs.OrdiniClienti.ApplicationServices.Handlers.Articoli
{
    public class DescrizioneArticoloModificataEventHandler : RequestHandler<DescrizioneArticoloModificata>
    {
        private readonly IArticoloFactory _factory;

        public DescrizioneArticoloModificataEventHandler(IArticoloFactory factory)
        {
            this._factory = factory;
        }

        public override DescrizioneArticoloModificata Handle(DescrizioneArticoloModificata command)
        {
            this._factory.ModificaDescrizioneArticoloAsync(command.ArticoloId, command.ArticoloDescrizione).GetAwaiter()
                .GetResult();

            return base.Handle(command);
        }
    }
}