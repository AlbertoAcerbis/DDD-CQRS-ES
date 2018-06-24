using System;

namespace Muflone.CommonDomain.Persistence
{
    public interface IConstructSagas
    {
        ISaga Build(Type type, string id);
    }
}