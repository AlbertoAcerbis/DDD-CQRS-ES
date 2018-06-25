using System;
using Autofac;
using FourSolid.Cqrs.Anagrafiche.ApplicationServices.Handlers.Articoli;
using FourSolid.Cqrs.Anagrafiche.ApplicationServices.Handlers.Clienti;
using FourSolid.Cqrs.Anagrafiche.ApplicationServices.Mappers.Articoli;
using FourSolid.Cqrs.Anagrafiche.ApplicationServices.Mappers.Clienti;
using FourSolid.Cqrs.Anagrafiche.Messages.Events;
using FourSolid.Cqrs.Anagrafiche.Shared.Configuration;
using Microsoft.Extensions.Options;
using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RMQ;
using Paramore.Brighter.MessagingGateway.RMQ.MessagingGatewayConfiguration;
using Paramore.Brighter.ServiceActivator;
using Polly;

namespace FourSolid.Cqrs.Anagrafiche.Mediator
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
            subscriberRegistry.Register<ArticoloCreated, ArticoloCreatedEventHandler>();

            subscriberRegistry.Register<ClienteCreated, ClienteCreatedEventHandler>();

            var messageMapperRegistry = new MessageMapperRegistry(messageMapperFactory)
            {
                { typeof(ArticoloCreated), typeof(ArticoloCreatedMapper) },

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
                    new Connection<ArticoloCreated>(new ConnectionName("ArticoloCreatedEvent"),
                        new ChannelName("ArticoloCreated"),
                        new RoutingKey("ArticoloCreated"),
                        timeoutInMilliseconds: 200),

                    new Connection<ClienteCreated>(new ConnectionName("ClienteCreatedEvent"),
                        new ChannelName("ClienteCreated"),
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