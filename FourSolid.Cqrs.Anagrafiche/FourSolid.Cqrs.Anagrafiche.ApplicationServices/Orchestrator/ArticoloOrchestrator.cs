using System.Collections.Generic;
using System.Threading.Tasks;
using FourSolid.Common.InProcessBus.Abstracts;
using FourSolid.Cqrs.Anagrafiche.Messages.Commands;
using FourSolid.Cqrs.Anagrafiche.Shared.ApplicationServices;
using FourSolid.Cqrs.Anagrafiche.Shared.JsonModel;
using FourSolid.Shared.InfoModel;
using FourSolid.Shared.ValueObjects;

namespace FourSolid.Cqrs.Anagrafiche.ApplicationServices.Orchestrator
{
    public class ArticoloOrchestrator : IArticoloOrchestrator
    {
        private readonly IServiceBus _serviceBus;
        private readonly IArticoloFactory _articoloFactory;

        public ArticoloOrchestrator(IServiceBus serviceBus, IArticoloFactory articoloFactory)
        {
            this._serviceBus = serviceBus;
            this._articoloFactory = articoloFactory;
        }

        public async Task CreateArticoloAsync(ArticoloJson articoloToCreate, AccountInfo who, When when)
        {
            var articoloId = new ArticoloId(articoloToCreate.ArticoloId);

            var createArticoloCommand = new CreateArticolo(articoloId,
                new ArticoloDescrizione(articoloToCreate.ArticoloDescrizione),
                new UnitaMisura(articoloToCreate.UnitaMisura), new ScortaMinima(articoloToCreate.ScortaMinima), 
                who, when);

            await this._serviceBus.SendAsync(createArticoloCommand);
        }

        public async Task<IEnumerable<ArticoloJson>> GetArticoliAsync() =>
            await this._articoloFactory.GetArticoliAsync();

        public async Task<ArticoloJson> GetArticoloByIdAsync(string articoloId) =>
            await this._articoloFactory.GetArticoloByIdAsync(new ArticoloId(articoloId));
    }
}