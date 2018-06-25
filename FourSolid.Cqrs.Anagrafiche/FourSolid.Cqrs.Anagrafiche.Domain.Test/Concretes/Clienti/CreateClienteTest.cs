using System;
using System.Collections.Generic;
using Autofac;
using Muflone.CommonDomain.Persistence;
using FourSolid.Cqrs.Anagrafiche.Domain.CommandsHandler.Clienti;
using FourSolid.Cqrs.Anagrafiche.Messages.Commands;
using FourSolid.Cqrs.Anagrafiche.Messages.Events;
using FourSolid.Shared.InfoModel;
using FourSolid.Shared.ValueObjects;
using Paramore.Brighter;
using Xunit;

namespace FourSolid.Cqrs.Anagrafiche.Domain.Test.Concretes.Clienti
{
    public class CreateClienteTest : EventSpecification<CreateCliente>
    {
        private readonly ClienteId _clienteId = new ClienteId(Guid.NewGuid().ToString());
        private readonly RagioneSociale _ragioneSociale = new RagioneSociale("RagioneSociale");
        private readonly PartitaIva _partitaIva = new PartitaIva("03466990177");
        private readonly CodiceFiscale _codiceFiscale = new CodiceFiscale("03466990177");

        private readonly AccountInfo _who = new AccountInfo(new AccountId(Guid.NewGuid().ToString("N")),
            new AccountName("Tester"), new AccountRole("T"));
        private readonly When _when = new When(DateTime.UtcNow);

        private readonly IContainer _container;

        public CreateClienteTest()
        {
            var builder = AutofacBootstrapper.BuilderContainer();
            this._container = builder.Build();
        }

        [Fact]
        private void Test()
        {
            var inMemoryRepository = this._container.Resolve<IRepository>() as InMemoryEventRepository;
            Assert.True(this.RunTest(inMemoryRepository), this.Caught != null ? this.Caught.Message : "");
        }

        protected override IEnumerable<Event> Given()
        {
            yield break;
        }

        protected override CreateCliente When()
        {
            return new CreateCliente(this._clienteId, this._ragioneSociale, this._partitaIva, this._codiceFiscale,
                this._who, this._when);
        }

        protected override RequestHandlerAsync<CreateCliente> OnHandler()
        {
            var createClienteCommandHandler = this._container.Resolve<CreateClienteCommandHandler>();
            return createClienteCommandHandler;
        }

        protected override IEnumerable<Event> Expect()
        {
            yield return new ClienteCreated(this._clienteId, this._ragioneSociale, this._partitaIva,
                this._codiceFiscale, this._who, this._when);
        }
    }
}