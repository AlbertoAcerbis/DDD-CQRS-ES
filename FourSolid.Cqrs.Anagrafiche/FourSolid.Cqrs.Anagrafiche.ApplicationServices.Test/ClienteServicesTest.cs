using System;
using System.Threading.Tasks;
using Autofac;
using FourSolid.Cqrs.Anagrafiche.Domain.Rules.Resources;
using FourSolid.Cqrs.Anagrafiche.Shared.ApplicationServices;
using FourSolid.Cqrs.Anagrafiche.Shared.JsonModel;
using FourSolid.Shared.InfoModel;
using FourSolid.Shared.Services;
using FourSolid.Shared.ValueObjects;
using Xunit;

namespace FourSolid.Cqrs.Anagrafiche.ApplicationServices.Test
{
    public class ClienteServicesTest
    {
        private readonly IContainer _container;

        private readonly AccountInfo _who = new AccountInfo(new AccountId(Guid.NewGuid().ToString()),
            new AccountName("Tester"), new AccountRole("T"));
        private readonly When _when = new When(DateTime.UtcNow);

        public ClienteServicesTest()
        {
            var builder = AutofacBootstrapper.RegisterModules();
            this._container = builder.Build();
        }

        [Fact]
        public async Task Cannot_CreateCliente_Without_ClienteRagioneSociale()
        {
            var clienteOrchestrator = this._container.Resolve<IClienteOrchestrator>();
            var clienteJson = new ClienteJson
            {
                ClienteId = string.Empty,
                RagioneSociale = string.Empty,
                PartitaIva = "03466990177",
                CodiceFiscale = "03466990177"
            };

            Exception ex = await Assert.ThrowsAnyAsync<Exception>(() =>
                clienteOrchestrator.CreateClienteAsync(clienteJson, this._who, this._when));

            Assert.Equal(DomainExceptions.RagioneSocialeNullException, CommonServices.GetErrorMessage(ex));
        }
    }
}