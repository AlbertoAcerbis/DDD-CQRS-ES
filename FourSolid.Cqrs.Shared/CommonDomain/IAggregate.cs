using System;
using System.Collections;

namespace Muflone.CommonDomain
{
    public interface IAggregate
    {
        Guid Id { get; }
        int Version { get; }

        void ApplyEvent(object @event);
        ICollection GetUncommittedEvents();
        void ClearUncommittedEvents();

        IMemento GetSnapshot();
    }
}