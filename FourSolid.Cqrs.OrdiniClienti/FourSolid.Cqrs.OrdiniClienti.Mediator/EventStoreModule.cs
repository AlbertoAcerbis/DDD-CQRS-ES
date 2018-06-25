using Autofac;
using EventStore.ClientAPI;
using FourSolid.Common.InProcessBus.Abstracts;
using Muflone.CommonDomain.Persistence;
using FourSolid.EventStore;
using FourSolid.EventStore.Configuration;

namespace FourSolid.Cqrs.OrdiniClienti.Mediator
{
    public class EventStoreModule : Module
    {
        private readonly EventStoreConfiguration _eventStoreConfiguration;
        public EventStoreModule(EventStoreConfiguration eventStoreConfiguration)
        {
            this._eventStoreConfiguration = eventStoreConfiguration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var eventStoreConnectionFactory = new EventStoreConnectionFactory(this._eventStoreConfiguration);
            IEventStoreConnection eventStoreConnection = eventStoreConnectionFactory.GetEventStoreConnection();
            eventStoreConnection.ConnectAsync().Wait();
            builder.RegisterInstance<IEventStoreConnection>(eventStoreConnection);

            var eventStoreRepository = new EventStoreRepository(eventStoreConnection);
            builder.RegisterInstance<IRepository>(eventStoreRepository).SingleInstance();

            builder.Register(r => new EventDispatcher(eventStoreConnection, r.Resolve<IEventBus>()))
                .As<EventDispatcher>()
                .SingleInstance();
        }
    }
}