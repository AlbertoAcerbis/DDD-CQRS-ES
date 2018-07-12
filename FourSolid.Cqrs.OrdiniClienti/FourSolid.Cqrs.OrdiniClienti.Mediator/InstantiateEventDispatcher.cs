using System;
using Autofac;
using FourSolid.Cqrs.OrdiniClienti.ApplicationServices.Handlers.Articoli;
using FourSolid.Cqrs.OrdiniClienti.ApplicationServices.Handlers.Clienti;
using FourSolid.Cqrs.OrdiniClienti.ApplicationServices.Handlers.OrdiniCliente;
using FourSolid.Cqrs.OrdiniClienti.ApplicationServices.Mappers.Articoli;
using FourSolid.Cqrs.OrdiniClienti.ApplicationServices.Mappers.Clienti;
using FourSolid.Cqrs.OrdiniClienti.ApplicationServices.Mappers.OrdiniCliente;
using FourSolid.Cqrs.OrdiniClienti.Messages.Events;
using FourSolid.Cqrs.OrdiniClienti.Shared.Configuration;
using Microsoft.Extensions.Options;
using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RMQ;
using Paramore.Brighter.MessagingGateway.RMQ.MessagingGatewayConfiguration;
using Paramore.Brighter.ServiceActivator;
using Polly;

namespace FourSolid.Cqrs.OrdiniClienti.Mediator
{
    public class InstantiateEventDispatcher : IDisposable
    {
        private Dispatcher _dispatcher;
        private CommandProcessor _commandProcessor;

        public void StartDispatcher(IContainer container)
        {
            var handlerFactory = container.Resolve<IAmAHandlerFactory>();
            var messageMapperFactory = container.Resolve<IAmAMessageMapperFactory>();

            var options = container.Resolve<IOptions<FourSettings>>();
            var authSettings = options.Value;

            var subscriberRegistry = new SubscriberRegistry();
            subscriberRegistry.Register<OrdineClienteCreated, OrdineClienteCreatedEventHandler>();

            subscriberRegistry.Register<ArticoloCreated, ArticoloCreatedEventHandler>();
            subscriberRegistry.Register<DescrizioneArticoloModificata, DescrizioneArticoloModificataEventHandler>();

            subscriberRegistry.Register<ClienteCreated, ClienteCreatedEventHandler>();

            var messageMapperRegistry = new MessageMapperRegistry(messageMapperFactory)
            {
                { typeof(OrdineClienteCreated), typeof(OrdineClienteCreatedMapper) },

                { typeof(ArticoloCreated), typeof(ArticoloCreatedMapper) },
                { typeof(DescrizioneArticoloModificata), typeof(DescrizioneArticoloModificataMapper) },

                { typeof(ClienteCreated), typeof(ClienteCreatedMapper) },
            };

            var handlerConfiguration = new HandlerConfiguration(subscriberRegistry, handlerFactory);

            var rmqConnnection = new RmqMessagingGatewayConnection
            {
                AmpqUri = new AmqpUriSpecification(new Uri(authSettings.RabbitMq.Uri)),
                Exchange = new Exchange(authSettings.RabbitMq.Events, "topic")
            };

            this._commandProcessor = CommandProcessorBuilder.With()
                .Handlers(handlerConfiguration)
                .Policies(PolicyRegistry())
                .NoTaskQueues()
                .RequestContextFactory(new InMemoryRequestContextFactory())
                .Build();

            this._dispatcher = DispatchBuilder.With()
                .CommandProcessor(this._commandProcessor)
                .MessageMappers(messageMapperRegistry)
                .DefaultChannelFactory(new InputChannelFactory(new RmqMessageConsumerFactory(rmqConnnection)))
                .Connections(new Connection[]
                {
                    new Connection<OrdineClienteCreated>(new ConnectionName("OrdineClienteCreatedEvent"),
                        new ChannelName("OrdineClienteCreated"),
                        new RoutingKey("OrdineClienteCreated"),
                        timeoutInMilliseconds: 200),

                    new Connection<ArticoloCreated>(new ConnectionName("ArticoloCreatedEvent"),
                        new ChannelName("ArticoloForOrdiniClientiCreated"),
                        new RoutingKey("ArticoloCreated"),
                        timeoutInMilliseconds: 200),
                    new Connection<DescrizioneArticoloModificata>(new ConnectionName("DescrizioneArticoloModificataEvent"),
                        new ChannelName("DescrizioneArticoloForOrdiniClientiModificata"),
                        new RoutingKey("DescrizioneArticoloModificata"),
                        timeoutInMilliseconds: 200),

                    new Connection<ClienteCreated>(new ConnectionName("ClienteCreatedEvent"),
                        new ChannelName("ClienteForOrdiniClientiCreated"),
                        new RoutingKey("ClienteCreated"),
                        timeoutInMilliseconds: 200),
                }).Build();

            this._dispatcher.Receive();
        }

        #region Helpers
        private static PolicyRegistry PolicyRegistry()
        {
            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(new[]
                {
                    TimeSpan.FromMilliseconds(50),
                    TimeSpan.FromMilliseconds(100),
                    TimeSpan.FromMilliseconds(150)
                });

            var circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreaker(1, TimeSpan.FromMilliseconds(500));

            var policyRegistry = new PolicyRegistry
            {
                {CommandProcessor.RETRYPOLICY, retryPolicy},
                {CommandProcessor.CIRCUITBREAKER, circuitBreakerPolicy}
            };
            return policyRegistry;
        }
        #endregion

        #region Dispose
        private bool _disposed;

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this._dispatcher?.End().Wait();
                    this._commandProcessor?.Dispose();
                }
            }
            this._disposed = true;
        }
        #endregion
    }
}