using System.Configuration;

namespace FourSolid.Cqrs.EventsDispatcher.Configuration
{
    public class EvenstDispatcherConfiguration
    {
        private const string CreditoItaliaGroupName = "FourSolid/";

        private static EventStoreSectionHandler _eventStoreSection;
        public static EventStoreSectionHandler EventStoreSection => _eventStoreSection ??
                                                                    (_eventStoreSection =
                                                                        ConfigurationManager.GetSection($"{CreditoItaliaGroupName}EventStore") as
                                                                            EventStoreSectionHandler);

        private static RabbitMqSectionHandler _rabbitMqSection;
        public static RabbitMqSectionHandler RabbitMqSection => _rabbitMqSection ?? (_rabbitMqSection =
                                                                    ConfigurationManager.GetSection(
                                                                        $"{CreditoItaliaGroupName}RabbitMq") as
                                                                        RabbitMqSectionHandler);

        private static MongoDbSectionHandler _mongoDbSection;
        public static MongoDbSectionHandler MongoDbSection => _mongoDbSection ?? (_mongoDbSection =
                                                                  ConfigurationManager.GetSection(
                                                                          $"{CreditoItaliaGroupName}MongoDb") as
                                                                      MongoDbSectionHandler);
    }
}