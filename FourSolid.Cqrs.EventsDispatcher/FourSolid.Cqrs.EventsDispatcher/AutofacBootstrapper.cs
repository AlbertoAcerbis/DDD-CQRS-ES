using Autofac;
using EventStore.ClientAPI;
using Muflone.CommonDomain.Persistence;
using FourSolid.Cqrs.EventsDispatcher.Configuration;
using FourSolid.Cqrs.EventsDispatcher.EventStore;
using FourSolid.Cqrs.EventsDispatcher.EventStore.Abstract;
using FourSolid.Cqrs.EventsDispatcher.EventStore.Concrete;
using FourSolid.Cqrs.EventsDispatcher.EventStore.Configuration;
using FourSolid.Cqrs.EventsDispatcher.EventStore.Repositories;
using FourSolid.Cqrs.EventsDispatcher.Logging.Abstracts;
using FourSolid.Cqrs.EventsDispatcher.Logging.Concretes;
using FourSolid.Cqrs.EventsDispatcher.MongoDb.Abstracts;
using FourSolid.Cqrs.EventsDispatcher.MongoDb.Repository;
using FourSolid.Cqrs.EventsDispatcher.MongoDb.Services;

namespace FourSolid.Cqrs.EventsDispatcher
{
    public class AutofacBootstrapper
    {
        internal static ContainerBuilder RegisterModules()
        {
            var eventStoreConfiguration = new EventStoreConfiguration(
                EvenstDispatcherConfiguration.EventStoreSection.Uri,
                EvenstDispatcherConfiguration.EventStoreSection.Port,
                EvenstDispatcherConfiguration.EventStoreSection.User,
                EvenstDispatcherConfiguration.EventStoreSection.Password,
                "",
                "");

            var rmqParameters = new RmqParameters
            (
                EvenstDispatcherConfiguration.RabbitMqSection.Uri,
                EvenstDispatcherConfiguration.RabbitMqSection.HostName,
                EvenstDispatcherConfiguration.RabbitMqSection.Username,
                EvenstDispatcherConfiguration.RabbitMqSection.Password,
                EvenstDispatcherConfiguration.RabbitMqSection.Events,
                24
            );

            var builder = new ContainerBuilder();
            builder.RegisterType<EventBus>().As<IEventBus>().SingleInstance();
            builder.RegisterModule(new RabbitMqModule(rmqParameters));
            builder.RegisterModule(new EventStoreModule(eventStoreConfiguration));
            builder.RegisterType<DocumentUnitOfWork>().As<IDocumentUnitOfWork>().InstancePerDependency();
            builder.RegisterType<LogService>().As<ILogService>().InstancePerDependency();
            builder.RegisterType<EventsFactory>().As<IEventsFactory>().InstancePerDependency();

            return builder;
        }
    }

    internal class EventStoreModule : Module
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
            builder.RegisterInstance(eventStoreConnection);

            var eventStoreRepository = new EventStoreRepository(eventStoreConnection);
            builder.RegisterInstance<IRepository>(eventStoreRepository).SingleInstance();

            builder.Register(r => new EventDispatcher(eventStoreConnection, r.Resolve<IEventBus>(), r.Resolve<ILogService>(), r.Resolve<IEventsFactory>()))
                .As<EventDispatcher>()
                .SingleInstance();
        }
    }

    internal class RabbitMqModule : Module
    {
        private readonly RmqParameters _rmqParameters;
        public RabbitMqModule(RmqParameters rmqParameters)
        {
            this._rmqParameters = rmqParameters;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(r => new BrokerClient(this._rmqParameters, r.Resolve<ILogService>())).As<IBrokerClient>()
                .SingleInstance();
        }
    }
}