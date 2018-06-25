namespace FourSolid.Cqrs.EventsDispatcher.EventStore.Abstract
{
    public interface IBrokerClient
    {
        void Send(dynamic body, string routingKey);
        void Receive();
    }
}