namespace FourSolid.EventStore.Configuration
{
    public class EventStoreConfiguration
    {
        public string Uri { get; }
        public int Port { get; }
        public string User { get; }
        public string Password { get; }
        public string EventClrTypeHeader { get; }
        public string AggregateClrTypeHeader { get; }

        public EventStoreConfiguration(string uri, int port, string user, string password, string eventClrTypeHeader,
            string aggregateClrTypeHeader)
        {
            this.Uri = uri;
            this.Port = port;
            this.User = user;
            this.Password = password;
            this.EventClrTypeHeader = eventClrTypeHeader;
            this.AggregateClrTypeHeader = aggregateClrTypeHeader;
        }
    }
}