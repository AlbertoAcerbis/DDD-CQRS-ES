using System;
using Autofac;
using Paramore.Brighter;

namespace FourSolid.Cqrs.OrdiniClienti.Mediator.Factory
{
    public class MessageMapperFactory : IAmAMessageMapperFactory
    {
        private readonly IComponentContext _componentContext;

        public MessageMapperFactory(IComponentContext componentContext)
        {
            this._componentContext = componentContext;
        }

        public IAmAMessageMapper Create(Type messageMapperType)
        {
            return this._componentContext.Resolve(messageMapperType) as IAmAMessageMapper;
        }
    }
}