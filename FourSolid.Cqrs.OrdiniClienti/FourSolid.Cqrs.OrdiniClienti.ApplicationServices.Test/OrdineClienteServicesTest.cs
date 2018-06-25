using System;
using System.Threading.Tasks;
using Autofac;
using FourSolid.Cqrs.OrdiniClienti.Domain.Rules.Resources;
using FourSolid.Cqrs.OrdiniClienti.Shared.ApplicationServices;
using FourSolid.Cqrs.OrdiniClienti.Shared.JsonModel;
using FourSolid.Shared.InfoModel;
using FourSolid.Shared.Services;
using FourSolid.Shared.ValueObjects;
using Xunit;

namespace FourSolid.Cqrs.OrdiniClienti.ApplicationServices.Test
{
    public class OrdineClienteServicesTest
    {
        private readonly IContainer _container;

        private readonly AccountInfo _who = new AccountInfo(new AccountId(Guid.NewGuid().ToString()),
            new AccountName("Tester"), new AccountRole("T"));
        private readonly When _when = new When(DateTime.UtcNow);

        public OrdineClienteServicesTest()
        {
            var builder = AutofacBootstrapper.RegisterModules();
            this._container = builder.Build();
        }

        [Fact]
        public async Task Cannot_CreateOrdineCliente_Without_ClienteId()
        {
            var ordineClienteOrchestrator = this._container.Resolve<IOrdineClienteOrchestrator>();
            var ordineCliente = new OrdineClienteJson
            {
                ClienteId = string.Empty,
                DataInserimento = DateTime.UtcNow,
                DataPrevistaConsegna = DateTime.UtcNow.AddMonths(3)
            };

            Exception ex = await Assert.ThrowsAnyAsync<Exception>(() =>
                ordineClienteOrchestrator.CreateOrdineClienteAsync(ordineCliente, this._who, this._when));

            Assert.Equal(DomainExceptions.ClienteIdNullException, CommonServices.GetErrorMessage(ex));
        }
    }
}
