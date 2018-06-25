using Autofac;
using FourSolid.Cqrs.OrdiniClienti.Mediator;
using FourSolid.Cqrs.OrdiniClienti.Shared.Configuration;
using FourSolid.EventStore.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FourSolid.Cqrs.OrdiniClienti.ApplicationServices.Test
{
    internal class AutofacBootstrapper
    {
        internal static ContainerBuilder RegisterModules()
        {
            var builder = new ContainerBuilder();

            #region ILogger
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            //var articoloServicesLogger = factory.CreateLogger<AffiliatoServices>();
            //var coordinatoreServicesLogger = factory.CreateLogger<CoordinatoreServices>();
            //builder.RegisterInstance(affiliatoServicesLogger);
            //builder.RegisterInstance(coordinatoreServicesLogger);
            #endregion

            #region IOptions
            IOptions<FourSettings> options = Options.Create(new FourSettings
            {
                EventStoreParameters = new EventStoreParameters
                {
                    Uri = "127.0.0.1",
                    Port = 1113,
                    User = "admin",
                    Password = "changeit"
                },
                MongoDbParameters = new MongoDbParameters
                {
                    ConnectionString = "mongodb://4Solid:4$olid!2016@cluster0-shard-00-00-5pgij.mongodb.net:27017,cluster0-shard-00-01-5pgij.mongodb.net:27017,cluster0-shard-00-02-5pgij.mongodb.net:27017/test?ssl=true&replicaSet=Cluster0-shard-0&authSource=admin"
                }
            });
            builder.RegisterInstance(options);
            #endregion

            var eventStoreConfiguration = new EventStoreConfiguration
            (
                options.Value.EventStoreParameters.Uri,
                options.Value.EventStoreParameters.Port,
                options.Value.EventStoreParameters.User,
                options.Value.EventStoreParameters.Password,
                "4SolidEvents", "4SolidAggregates"
            );
            builder.RegisterModule(new EventStoreModule(eventStoreConfiguration));

            builder.RegisterModule<InProcessBusModule>();

            builder.RegisterModule<FactoryModule>();
            builder.RegisterModule<CommandsModule>();
            builder.RegisterModule<MappersModule>();
            builder.RegisterModule<EventsModule>();
            builder.RegisterModule<InstantiateCommandProcessor>();

            builder.RegisterModule<ServicesModule>();

            return builder;
        }
    }
}