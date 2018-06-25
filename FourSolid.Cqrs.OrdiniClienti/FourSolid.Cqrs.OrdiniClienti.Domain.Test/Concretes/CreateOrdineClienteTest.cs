using System;
using System.Collections.Generic;
using Autofac;
using Muflone.CommonDomain.Persistence;
using FourSolid.Cqrs.OrdiniClienti.Domain.CommandsHandler;
using FourSolid.Cqrs.OrdiniClienti.Messages.Commands;
using FourSolid.Cqrs.OrdiniClienti.Messages.Events;
using FourSolid.Shared.InfoModel;
using FourSolid.Shared.ValueObjects;
using Paramore.Brighter;
using Xunit;

namespace FourSolid.Cqrs.OrdiniClienti.Domain.Test.Concretes
{
    public class CreateOrdineClienteTest : EventSpecification<CreateOrdineCliente>
    {
        private readonly OrdineClienteId _ordineClienteId = new OrdineClienteId(Guid.NewGuid().ToString());
        private readonly ClienteId _clienteId = new ClienteId(Guid.NewGuid().ToString());
        private readonly DataInserimento _dataInserimento = new DataInserimento(DateTime.UtcNow);
        private readonly DataPrevistaConsegna _dataPrevistaConsegna =
            new DataPrevistaConsegna(DateTime.UtcNow.AddMonths(3));
            
        private readonly AccountInfo _who = new AccountInfo(new AccountId(Guid.NewGuid().ToString("N")),
            new AccountName("Tester"), new AccountRole("T"));
        private readonly When _when = new When(DateTime.UtcNow);

        private readonly IContainer _container;

        public CreateOrdineClienteTest()
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

        protected override CreateOrdineCliente When()
        {
            return new CreateOrdineCliente(this._ordineClienteId, this._clienteId, this._dataInserimento,
                this._dataPrevistaConsegna, this._who, this._when);
        }

        protected override RequestHandlerAsync<CreateOrdineCliente> OnHandler()
        {
            var createOrdineClienteCommandHandler = this._container.Resolve<CreateOrdineClienteCommandHandler>();
            return createOrdineClienteCommandHandler;
        }

        protected override IEnumerable<Event> Expect()
        {
            yield return new OrdineClienteCreated(this._ordineClienteId, this._clienteId, this._dataInserimento,
                this._dataPrevistaConsegna, this._who, this._when);
        }
    }
}