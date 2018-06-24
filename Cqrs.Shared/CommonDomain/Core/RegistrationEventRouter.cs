using System;
using System.Collections.Generic;

namespace Muflone.CommonDomain.Core
{
    public class RegistrationEventRouter : IRouteEvents
    {
        private readonly IDictionary<Type, Action<object>> _handlers = new Dictionary<Type, Action<object>>();

        private IAggregate _regsitered;

        public virtual void Register<T>(Action<T> handler)
        {
            this._handlers[typeof(T)] = @event => handler((T)@event);
        }

        public virtual void Register(IAggregate aggregate)
        {
            this._regsitered = aggregate ?? throw new ArgumentNullException("aggregate");
        }

        public virtual void Dispatch(object eventMessage)
        {
            if (!this._handlers.TryGetValue(eventMessage.GetType(), out var handler))
                    this._regsitered.ThrowHandlerNotFound(eventMessage);
            handler(eventMessage);
        }
    }
}