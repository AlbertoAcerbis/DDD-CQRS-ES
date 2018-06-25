using System.Net;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using FourSolid.EventStore.Configuration;

namespace FourSolid.EventStore
{
    public class EventStoreConnectionFactory
    {
        private static IEventStoreConnection _eventStoreConnection;
        private readonly EventStoreConfiguration _eventStoreConfiguration;

        public EventStoreConnectionFactory(EventStoreConfiguration eventStoreConfiguration)
        {
            this._eventStoreConfiguration = eventStoreConfiguration;
        }

        public IEventStoreConnection GetEventStoreConnection()
        {
            return _eventStoreConnection ??
                   (_eventStoreConnection = this.CreateEventStoreConnection());
        }

        private IEventStoreConnection CreateEventStoreConnection()
        {
            var ipEndPoint =
                new IPEndPoint(IPAddress.Parse(this._eventStoreConfiguration.Uri),
                    this._eventStoreConfiguration.Port);

            var connectionSettings = ConnectionSettings.Create();
            connectionSettings.SetDefaultUserCredentials(
                new UserCredentials(this._eventStoreConfiguration.User,
                    this._eventStoreConfiguration.Password));

            return EventStoreConnection.Create(connectionSettings, ipEndPoint);
        }
    }
}