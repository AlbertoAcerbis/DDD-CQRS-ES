namespace FourSolid.Cqrs.EventsDispatcher.EventStore.Configuration
{
    public class RmqParameters
    {
        public string Uri { get; }
        public string HostName { get; }
        public string Username { get; }
        public string Password { get; }
        public string Exchange { get; }
        public int TimeToLive { get; }

        public RmqParameters(string uri, string hostname, string username, string password, string exchange, int timeToLive)
        {
            this.Uri = uri;
            this.HostName = hostname;
            this.Username = username;
            this.Password = password;
            this.Exchange = exchange;
            this.TimeToLive = timeToLive;
        }
    }
}