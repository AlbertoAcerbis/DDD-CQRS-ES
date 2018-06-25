using System.Threading.Tasks;
using FourSolid.Cqrs.EventsDispatcher.EventStore.Abstract;

namespace FourSolid.Cqrs.EventsDispatcher.EventStore.Concrete
{
    public class EventBus : IEventBus
    {
        private readonly IBrokerClient _brokerClient;

        public EventBus(IBrokerClient brokerClient)
        {
            this._brokerClient = brokerClient;
        }

        public Task PublishAsync(dynamic @event, string routingKey)
        {
            this._brokerClient.Send(@event, routingKey);

            return Task.CompletedTask;
        }
    }
}