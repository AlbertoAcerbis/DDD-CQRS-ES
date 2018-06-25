using System;
using Autofac;
using Paramore.Brighter;

namespace FourSolid.Cqrs.OrdiniClienti.Mediator.Factory
{
    public class ServicesHandlerFactoryAsync : IAmAHandlerFactoryAsync
    {
        private readonly IComponentContext _componentContext;

        public ServicesHandlerFactoryAsync(IComponentContext componentContext)
        {
            this._componentContext = componentContext;
        }

        public IHandleRequestsAsync Create(Type handlerType)
        {
            return this._componentContext.Resolve(handlerType) as IHandleRequestsAsync;
        }

        public void Release(IHandleRequestsAsync handler)
        {
            var disposable = handler as IDisposable;
            disposable?.Dispose();
        }
    }
}