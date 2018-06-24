using System;

namespace Muflone.CommonDomain
{
    public interface IMemento
    {
        Guid Id { get; set; }

        int Version { get; set; }
    }
}