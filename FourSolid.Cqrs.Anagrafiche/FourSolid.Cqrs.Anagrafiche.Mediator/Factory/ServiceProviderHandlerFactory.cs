using System;
using Autofac;
using Paramore.Brighter;

namespace FourSolid.Cqrs.Anagrafiche.Mediator.Factory
{
    public class ServiceProviderHandlerFactory : IAmAHandlerFactory
    {
        private readonly IComponentContext _componentContext;

        public ServiceProviderHandlerFactory(IComponentContext componentContext)
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