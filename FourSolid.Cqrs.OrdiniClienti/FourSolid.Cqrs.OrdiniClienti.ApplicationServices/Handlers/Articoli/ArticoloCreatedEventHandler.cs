using FourSolid.Cqrs.OrdiniClienti.Messages.Events;
using FourSolid.Cqrs.OrdiniClienti.Shared.ApplicationServices;
using Paramore.Brighter;

namespace FourSolid.Cqrs.OrdiniClienti.ApplicationServices.Handlers.Articoli
{
    public class ArticoloCreatedEventHandler : RequestHandler<ArticoloCreated>
    {
        private readonly IArticoloFactory _articoloFactory;

        public ArticoloCreatedEventHandler(IArticoloFactory articoloFactory)
        {
            this._articoloFactory = articoloFactory;
        }

        public override ArticoloCreated Handle(ArticoloCreated command)
        {
            this._articoloFactory.CreateArticoloAsync(command.ArticoloId, command.ArticoloDescrizione);

            return base.Handle(command);
        }
    }
}