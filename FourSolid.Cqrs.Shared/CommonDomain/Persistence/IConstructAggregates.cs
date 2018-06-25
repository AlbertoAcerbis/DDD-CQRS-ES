using System;

namespace Muflone.CommonDomain.Persistence
{
    public interface IConstructAggregates
	{
		IAggregate Build(Type type, Guid id, IMemento snapshot);
	}
}