using Autofac;
using FourSolid.Cqrs.OrdiniClienti.Mediator;
using FourSolid.EventStore.Configuration;
using Microsoft.Extensions.Configuration;

namespace FourSolid.Cqrs.OrdiniClienti
{
    internal class AutofacBootstrapper
    {
        internal static ContainerBuilder RegisterModules(IConfiguration configuration)
        {
            //TODO: I don't know if it's the best practice, but it works, and it's enough for me!
            var eventStoreConfiguration = new EventStoreConfiguration
            (
                configuration["4Solid:EventStoreParameters:Uri"],
                int.Parse(configuration["4Solid:EventStoreParameters:Port"]),
                configuration["4Solid:EventStoreParameters:User"],
                configuration["4Solid:EventStoreParameters:Password"],
                configuration["4Solid:EventStoreParameters:EventClrTypeHeader"],
                configuration["4Solid:EventStoreParameters:AggregateClrTypeHeader"]
            );

            var builder = new ContainerBuilder();
            builder.RegisterModule(new EventStoreModule(eventStoreConfiguration));

            builder.RegisterModule<InProcessBusModule>();

            builder.RegisterModule<FactoryModule>();
            builder.RegisterModule<CommandsModule>();
            builder.RegisterModule<MappersModule>();
            builder.RegisterModule<EventsModule>();
            builder.RegisterModule<InstantiateCommandProcessor>();

            builder.RegisterModule<ServicesModule>();

            builder.RegisterModule<ReadModelModule>();

            return builder;
        }
    }
}