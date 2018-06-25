namespace FourSolid.Cqrs.Anagrafiche.Shared.Configuration
{
    public class FourSettings
    {
        public TokenAuthentication TokenAuthentication { get; set; }
        public EventStoreParameters EventStoreParameters { get; set; }
        public RabbitMq RabbitMq { get; set; }
        public MongoDbParameters MongoDbParameters { get; set; }
    }

    public class TokenAuthentication
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string TokenPath { get; set; }
        public int TokenExpiration { get; set; }
        public string CookieName { get; set; }
    }

    public class EventStoreParameters
    {
        public string Uri { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string EventClrTypeHeader { get; set; }
        public string AggregateClrTypeHeader { get; set; }
    }

    public class RabbitMq
    {
        public string Uri { get; set; }
        public string HostName { get; set; }
        public string Commands { get; set; }
        public string Events { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class MongoDbParameters
    {
        public string ConnectionString { get; set; }
    }
}