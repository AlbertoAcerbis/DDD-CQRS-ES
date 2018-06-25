using System;
using System.Threading.Tasks;
using FourSolid.Common.InProcessBus.Abstracts;
using FourSolid.Cqrs.OrdiniClienti.Domain.Rules.Resources;
using FourSolid.Cqrs.OrdiniClienti.Messages.Commands;
using FourSolid.Cqrs.OrdiniClienti.Shared.ApplicationServices;
using FourSolid.Cqrs.OrdiniClienti.Shared.JsonModel;
using FourSolid.Shared.InfoModel;
using FourSolid.Shared.ValueObjects;

namespace FourSolid.Cqrs.OrdiniClienti.ApplicationServices.Orchestrator
{
    public class OrdineClienteOrchestrator : IOrdineClienteOrchestrator
    {
        private readonly IServiceBus _serviceBus;

        public OrdineClienteOrchestrator(IServiceBus serviceBus)
        {
            this._serviceBus = serviceBus;
        }

        public async Task CreateOrdineClienteAsync(OrdineClienteJson ordineCliente, AccountInfo who, When when)
        {
            if (string.IsNullOrEmpty(ordineCliente.ClienteId))
                throw new ArgumentException(DomainExceptions.ClienteIdNullException);

            var createOrdneClienteCommand = new CreateOrdineCliente(new OrdineClienteId(ordineCliente.ClienteId),
                new ClienteId(ordineCliente.ClienteId), new DataInserimento(ordineCliente.DataInserimento),
                new DataPrevistaConsegna(ordineCliente.DataPrevistaConsegna), who, when);

            await this._serviceBus.SendAsync(createOrdneClienteCommand);
        }
    }
}