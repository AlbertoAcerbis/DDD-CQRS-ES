using System;
using System.Collections;
using System.Collections.Generic;

namespace Muflone.CommonDomain.Core
{
    public abstract class AggregateBase : IAggregate, IEquatable<IAggregate>
    {
        private readonly ICollection<object> _uncommittedEvents = new LinkedList<object>();

        private IRouteEvents _registeredRoutes;

        protected AggregateBase()
            : this(null)
        { }

        protected AggregateBase(IRouteEvents handler)
        {
            if (handler == null)
            {
            return;
            }

            this.RegisteredRoutes = handler;
            this.RegisteredRoutes.Register(this);
        }

        protected IRouteEvents RegisteredRoutes
        {
            get => _registeredRoutes ?? (_registeredRoutes = new ConventionEventRouter(true, this));
            set => this._registeredRoutes = value ?? throw new InvalidOperationException("AggregateBase must have an event router to function");
        }

        public Guid Id { get; protected set; }

        public int Version { get; protected set; }

        void IAggregate.ApplyEvent(object @event)
        {
            this.RegisteredRoutes.Dispatch(@event);
            this.Version++;
        }

        ICollection IAggregate.GetUncommittedEvents()
        {
            return (ICollection)this._uncommittedEvents;
        }

        void IAggregate.ClearUncommittedEvents()
        {
            this._uncommittedEvents.Clear();
        }

        IMemento IAggregate.GetSnapshot()
        {
            IMemento snapshot = GetSnapshot();
            snapshot.Id = this.Id;
            snapshot.Version = this.Version;
            return snapshot;
        }

        public virtual bool Equals(IAggregate other)
        {
            return null != other && other.Id == this.Id;
        }

        protected void Register<T>(Action<T> route)
        {
            this.RegisteredRoutes.Register(route);
        }

        protected void RaiseEvent(object @event)
        {
            ((IAggregate)this).ApplyEvent(@event);
            this._uncommittedEvents.Add(@event);
        }

        protected virtual IMemento GetSnapshot()
        {
            return null;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as IAggregate);
        }
    }
}