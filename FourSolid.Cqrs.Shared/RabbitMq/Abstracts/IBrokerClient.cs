namespace FourSolid.MessagingGateway.RabbitMq.Abstracts
{
    public interface IBrokerClient
    {
        void Send(dynamic body, string routingKey);
        void Receive();
    }
}