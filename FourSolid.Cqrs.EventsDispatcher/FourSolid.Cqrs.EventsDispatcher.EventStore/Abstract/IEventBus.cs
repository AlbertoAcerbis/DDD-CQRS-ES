using System.Threading.Tasks;

namespace FourSolid.Cqrs.EventsDispatcher.EventStore.Abstract
{
    public interface IEventBus
    {
        Task PublishAsync(dynamic @event, string routingKey);
    }
}