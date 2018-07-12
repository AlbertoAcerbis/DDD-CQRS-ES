using System;
using System.Collections.Generic;
using Autofac;
using FourSolid.Cqrs.Anagrafiche.Domain.CommandsHandler.Articoli;
using FourSolid.Cqrs.Anagrafiche.Messages.Commands;
using FourSolid.Cqrs.Anagrafiche.Messages.Events;
using FourSolid.Shared.InfoModel;
using FourSolid.Shared.ValueObjects;
using Muflone.CommonDomain.Persistence;
using Paramore.Brighter;
using Xunit;

namespace FourSolid.Cqrs.Anagrafiche.Domain.Test.Concretes.Articoli
{
    public class ModificaDescrizioneArticoloTest : EventSpecification<ModificaDescrizioneArticolo>
    {
        private readonly ArticoloId _articoloId = new ArticoloId(Guid.NewGuid().ToString("N"));
        private readonly ArticoloDescrizione _articoloDescrizione = new ArticoloDescrizione("Descrizione");
        private readonly UnitaMisura _unitaMisura = new UnitaMisura("KG");
        private readonly ScortaMinima _scortaMinima = new ScortaMinima(15.5);

        private readonly AccountInfo _who = new AccountInfo(new AccountId(Guid.NewGuid().ToString("N")),
            new AccountName("Tester"), new AccountRole("T"));
        private readonly When _when = new When(DateTime.UtcNow);

        private readonly IContainer _container;

        public ModificaDescrizioneArticoloTest()
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
            yield return new ArticoloCreated(this._articoloId, this._articoloDescrizione, this._unitaMisura,
                this._scortaMinima, this._who, this._when);
        }

        protected override ModificaDescrizioneArticolo When()
        {
            return new ModificaDescrizioneArticolo(this._articoloId, this._articoloDescrizione, this._who, this._when);
        }

        protected override RequestHandlerAsync<ModificaDescrizioneArticolo> OnHandler()
        {
            var modificaDescrizioneHandler = this._container.Resolve<ModificaDescrizioneArticoloCommandHandler>();
            return modificaDescrizioneHandler;
        }

        protected override IEnumerable<Event> Expect()
        {
            yield return new DescrizioneArticoloModificata(this._articoloId, this._articoloDescrizione, this._who,
                this._when);
        }
    }
}