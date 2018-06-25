using FourSolid.Cqrs.Anagrafiche.ApplicationServices.Hubs;
using FourSolid.Cqrs.Anagrafiche.Messages.Events;
using FourSolid.Cqrs.Anagrafiche.Shared.ApplicationServices;
using Microsoft.AspNetCore.SignalR;
using Paramore.Brighter;

namespace FourSolid.Cqrs.Anagrafiche.ApplicationServices.Handlers.Articoli
{
    public class ArticoloCreatedEventHandler : RequestHandler<ArticoloCreated>
    {
        private readonly IArticoloFactory _articoloFactory;
        private readonly IHubContext<ArticoliHub> _articoliContext;

        public ArticoloCreatedEventHandler(IArticoloFactory articoloFactory, 
            IHubContext<ArticoliHub> articoliContext)
        {
            this._articoloFactory = articoloFactory;
            this._articoliContext = articoliContext;
        }

        public override ArticoloCreated Handle(ArticoloCreated command)
        {
            this._articoloFactory
                .CreateArticoloAsync(command.ArticoloId, command.ArticoloDescrizione, command.UnitaMisura,
                    command.ScortaMinima).GetAwaiter().GetResult();

            this._articoliContext.Clients.All.SendAsync("ArticoloCreated", command.AggregateId.ToString());

            return base.Handle(command);
        }
    }
}