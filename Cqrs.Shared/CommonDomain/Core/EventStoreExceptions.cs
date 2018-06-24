using System;

namespace Muflone.CommonDomain.Core
{
    public class AggregateDeletedException : Exception
    {
        public readonly Guid Id;
        public readonly Type Type;

        public AggregateDeletedException(Guid id, Type type)
            : base($"Aggregate '{id}' (type {type.Name}) was deleted.")
        {
                this.Id = id;
                this.Type = type;
        }
    }

    public class AggregateNotFoundException : Exception
    {
        public readonly Guid Id;
        public readonly Type Type;

        public AggregateNotFoundException(Guid id, Type type)
            : base($"Aggregate '{id}' (type {type.Name}) was not found.")
        {
            this.Id = id;
            this.Type = type;
        }
    }

    public class AggregateVersionException : Exception
    {
        public readonly Guid Id;
        public readonly Type Type;
        public readonly int AggregateVersion;
        public readonly int RequestedVersion;

        public AggregateVersionException(Guid id, Type type, int aggregateVersion, int requestedVersion)
            : base($"Requested version {requestedVersion} of aggregate '{id}' (type {type.Name}) - aggregate version is {aggregateVersion}")
        {
            this.Id = id;
            this.Type = type;
            this.AggregateVersion = aggregateVersion;
            this.RequestedVersion = requestedVersion;
        }
    }
}
