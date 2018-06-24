using System;
using System.Collections;
using System.Collections.Generic;

namespace Muflone.CommonDomain.Core
{
    public class SagaBase<TMessage> : ISaga, IEquatable<ISaga> where TMessage : class
    {
        private readonly IDictionary<Type, Action<TMessage>> _handlers = new Dictionary<Type, Action<TMessage>>();

        private readonly ICollection<TMessage> _uncommitted = new LinkedList<TMessage>();

        private readonly ICollection<TMessage> _undispatched = new LinkedList<TMessage>();

        public virtual bool Equals(ISaga other)
        {
            return null != other && other.Id == this.Id;
        }

        public string Id { get; protected set; }

        public int Version { get; private set; }

        public void Transition(object message)
        {
            this._handlers[message.GetType()](message as TMessage);
            this._uncommitted.Add(message as TMessage);
            this.Version++;
        }

        ICollection ISaga.GetUncommittedEvents()
        {
            return this._uncommitted as ICollection;
        }

        void ISaga.ClearUncommittedEvents()
        {
            this._uncommitted.Clear();
        }

        ICollection ISaga.GetUndispatchedMessages()
        {
            return this._undispatched as ICollection;
        }

        void ISaga.ClearUndispatchedMessages()
        {
            this._undispatched.Clear();
        }

        protected void Register<TRegisteredMessage>(Action<TRegisteredMessage> handler)
            where TRegisteredMessage : class, TMessage
        {
            this._handlers[typeof(TRegisteredMessage)] = message => handler(message as TRegisteredMessage);
        }

        protected void Dispatch(TMessage message)
        {
            this._undispatched.Add(message);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ISaga);
        }
    }
}