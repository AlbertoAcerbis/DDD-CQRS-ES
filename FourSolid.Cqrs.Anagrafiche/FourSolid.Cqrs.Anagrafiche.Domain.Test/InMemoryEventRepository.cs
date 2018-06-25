using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Muflone.CommonDomain;
using Muflone.CommonDomain.Persistence;
using FourSolid.EventStore;
using Paramore.Brighter;

namespace FourSolid.Cqrs.Anagrafiche.Domain.Test
{
    /// <summary>
    /// https://github.com/luizdamim/NEventStoreExample/tree/master/NEventStoreExample.Test
    /// </summary>
    public class InMemoryEventRepository : IRepository
    {
        private List<Event> _givenEvents;

        public InMemoryEventRepository(List<Event> givenEvents)
        {
            this._givenEvents = givenEvents;
        }

        public void SetGivenEvents(List<Event> givenEvents)
        {
            this._givenEvents = givenEvents;
        }

        public List<Event> Events { get; private set; }

        public async Task<TAggregate> GetByIdAsync<TAggregate>(Guid id) where TAggregate : class, IAggregate
        {
            return await this.GetByIdAsync<TAggregate>(id, 0);
        }

        public async Task<TAggregate> GetByIdAsync<TAggregate>(Guid id, int version) where TAggregate : class, IAggregate
        {
            return await this.GetByIdAsync<TAggregate>("BucketDefault", id, 0);
        }

        public async Task<TAggregate> GetByIdAsync<TAggregate>(string bucketId, Guid id, int version) where TAggregate : class, IAggregate
        {
            var aggregate = EventStoreRepository.ConstructAggregate<TAggregate>();
            await Task.Run(() => this._givenEvents.ForEach(aggregate.ApplyEvent));

            return aggregate;
        }

        public async Task SaveAsync(IAggregate aggregate, Guid commitId, Action<IDictionary<string, object>> updateHeaders)
        {
            await this.SaveAsync("BucketDefault", aggregate, commitId, updateHeaders);
        }

        public async Task SaveAsync(IAggregate aggregate, Guid commitId)
        {
            await this.SaveAsync(aggregate, commitId, d => { });
        }

        public Task SaveAsync(string bucketId, IAggregate aggregate, Guid commitId, Action<IDictionary<string, object>> updateHeaders)
        {
            this.Events = aggregate.GetUncommittedEvents().Cast<Event>().ToList();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            // no op
        }
    }
}