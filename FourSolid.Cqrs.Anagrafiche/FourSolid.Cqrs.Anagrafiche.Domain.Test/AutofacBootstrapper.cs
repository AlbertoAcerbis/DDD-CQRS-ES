using System;
using System.Collections.Generic;
using Autofac;
using Muflone.CommonDomain.Persistence;
using FourSolid.Cqrs.Anagrafiche.Domain.CommandsHandler.Articoli;
using FourSolid.Cqrs.Anagrafiche.Domain.CommandsHandler.Clienti;
using FourSolid.Cqrs.Anagrafiche.Messages.Commands;
using Paramore.Brighter;

namespace FourSolid.Cqrs.Anagrafiche.Domain.Test
{
    public class AutofacBootstrapper
    {
        internal static ContainerBuilder BuilderContainer()
        {
            var builder = new ContainerBuilder();

            #region CommandHandlers
            builder.RegisterType<CreateArticoloCommandHandler>().As<IHandleRequestsAsync<CreateArticolo>>().AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<CreateClienteCommandHandler>().As<IHandleRequestsAsync<CreateCliente>>().AsSelf()
                .InstancePerLifetimeScope();
            #endregion

            var commandStore = new InMemoryCommandStore();
            builder.RegisterInstance<IAmACommandStore>(commandStore).AsSelf().SingleInstance();

            var eventStoreRepository = new InMemoryEventRepository(new List<Event>());
            builder.RegisterInstance<IRepository>(eventStoreRepository).SingleInstance();

            return builder;
        }
    }

    public class AutofacIocHandlerFactory : IAmAHandlerFactory
    {
        private readonly IComponentContext _componentContext;

        public AutofacIocHandlerFactory(IComponentContext componentContext)
        {
            this._componentContext = componentContext;
        }

        public IHandleRequests Create(Type handlerType)
        {
            return this._componentContext.Resolve(handlerType) as IHandleRequests;
        }

        public void Release(IHandleRequests handler)
        {
            var disposable = handler as IDisposable;
            disposable?.Dispose();
        }
    }
}