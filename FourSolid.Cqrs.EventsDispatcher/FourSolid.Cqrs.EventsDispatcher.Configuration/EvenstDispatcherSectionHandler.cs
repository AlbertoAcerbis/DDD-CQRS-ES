using System.Configuration;

namespace FourSolid.Cqrs.EventsDispatcher.Configuration
{
    public class MongoDbSectionHandler : ConfigurationSection
    {
        [ConfigurationProperty("connectionString", DefaultValue = "mongodb://4Solid:4$olid!2016@cluster0-shard-00-00-5pgij.mongodb.net:27017,cluster0-shard-00-01-5pgij.mongodb.net:27017,cluster0-shard-00-02-5pgij.mongodb.net:27017/test?ssl=true&replicaSet=Cluster0-shard-0&authSource=admin", IsRequired = true)]
        public string ConnectionString
        {
            get => (string)this["connectionString"];
            set => this["connectionString"] = value;
        }

        [ConfigurationProperty("replicaSet", DefaultValue = "replicaSet=Cluster0-shard-0", IsRequired = true)]
        public string ReplicaSet
        {
            get => (string)this["replicaSet"];
            set => this["replicaSet"] = value;
        }

        [ConfigurationProperty("authSource", DefaultValue = "authSource=admin", IsRequired = true)]
        public string AuthSource
        {
            get => (string)this["authSource"];
            set => this["authSource"] = value;
        }
    }

    public class EventStoreSectionHandler : ConfigurationSection
    {
        [ConfigurationProperty("uri", DefaultValue = "127.0.0.1", IsRequired = true)]
        public string Uri
        {
            get => (string)this["uri"];
            set => this["uri"] = value;
        }

        [ConfigurationProperty("port", DefaultValue = 1113, IsRequired = true)]
        public int Port
        {
            get => (int)this["port"];
            set => this["port"] = value;
        }

        [ConfigurationProperty("user", DefaultValue = "admin", IsRequired = true)]
        public string User
        {
            get => (string)this["user"];
            set => this["user"] = value;
        }

        [ConfigurationProperty("password", DefaultValue = "changeit", IsRequired = true)]
        public string Password
        {
            get => (string)this["password"];
            set => this["password"] = value;
        }
    }

    public class RabbitMqSectionHandler : ConfigurationSection
    {
        [ConfigurationProperty("hostName", DefaultValue = "localhost", IsRequired = true)]
        public string HostName
        {
            get => (string)this["hostName"];
            set => this["hostName"] = value;
        }

        [ConfigurationProperty("uri", DefaultValue = "amqp://guest:guest@localhost:5672/", IsRequired = true)]
        public string Uri
        {
            get => (string)this["uri"];
            set => this["uri"] = value;
        }

        [ConfigurationProperty("commands", DefaultValue = "creditoitalia.exchange.commands", IsRequired = true)]
        public string Commands
        {
            get => (string)this["commands"];
            set => this["commands"] = value;
        }

        [ConfigurationProperty("events", DefaultValue = "creditoitalia.exchange.events", IsRequired = true)]
        public string Events
        {
            get => (string)this["events"];
            set => this["events"] = value;
        }

        [ConfigurationProperty("username", DefaultValue = "guest", IsRequired = true)]
        public string Username
        {
            get => (string)this["username"];
            set => this["username"] = value;
        }

        [ConfigurationProperty("password", DefaultValue = "guest", IsRequired = true)]
        public string Password
        {
            get => (string)this["password"];
            set => this["password"] = value;
        }
    }
}