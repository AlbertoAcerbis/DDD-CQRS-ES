using System;
using Autofac;
using Paramore.Brighter;

namespace FourSolid.Cqrs.Anagrafiche.Mediator.Factory
{
    public class ServicesHandlerFactory : IAmAHandlerFactory
    {
        private readonly IContainer _container;

        public ServicesHandlerFactory(IContainer containerBuilder)
        {
            this._container = containerBuilder;
        }

        public IHandleRequests Create(Type handlerType)
        {
            return _container.Resolve(handlerType) as IHandleRequests;
        }

        public void Release(IHandleRequests handler)
        {
            var disposable = handler as IDisposable;
            disposable?.Dispose();
        }
    }
}