using System;
using System.Collections.Generic;
using Autofac;
using Muflone.CommonDomain.Persistence;
using FourSolid.Cqrs.OrdiniClienti.Domain.CommandsHandler;
using FourSolid.Cqrs.OrdiniClienti.Messages.Commands;
using Paramore.Brighter;

namespace FourSolid.Cqrs.OrdiniClienti.Domain.Test
{
    public class AutofacBootstrapper
    {
        internal static ContainerBuilder BuilderContainer()
        {
            var builder = new ContainerBuilder();

            #region CommandHandlers
            builder.RegisterType<CreateOrdineClienteCommandHandler>().As<IHandleRequestsAsync<CreateOrdineCliente>>().AsSelf()
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