using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Muflone.CommonDomain.Persistence
{
    public interface IRepository : IDisposable
    {
        Task<TAggregate> GetByIdAsync<TAggregate>(Guid id) where TAggregate : class, IAggregate;
        Task<TAggregate> GetByIdAsync<TAggregate>(Guid id, int version) where TAggregate : class, IAggregate;
        Task SaveAsync(IAggregate aggregate, Guid commitId, Action<IDictionary<string, object>> updateHeaders);
        Task SaveAsync(IAggregate aggregate, Guid commitId);

        //Task SaveAsync(string bucketId, IAggregate aggregate, Guid commitId, Action<IDictionary<string, object>> updateHeaders);
        //Task<TAggregate> GetByIdAsync<TAggregate>(string bucketId, Guid id, int version) where TAggregate : class, IAggregate;

        //TAggregate GetById<TAggregate>(IIdentity id) where TAggregate : class, IAggregate;
        //TAggregate GetById<TAggregate>(IIdentity id, int version) where TAggregate : class, IAggregate;
        //Task<TAggregate> GetByIdAsync<TAggregate>(string bucketId, Guid id) where TAggregate : class, IAggregate;
        //TAggregate GetById<TAggregate>(string bucketId, IIdentity id, int version) where TAggregate : class, IAggregate;
        //void Save(IAggregate aggregate, Guid commitId, Action<IDictionary<string, object>> updateHeaders);
        //void Save(string bucketId, IAggregate aggregate, Guid commitId, Action<IDictionary<string, object>> updateHeaders);
    }
}