using System;
using FourSolid.Cqrs.EventsDispatcher.Configuration;
using FourSolid.Cqrs.EventsDispatcher.MongoDb.Abstracts;
using MongoDB.Driver;

namespace FourSolid.Cqrs.EventsDispatcher.MongoDb.Repository
{
    public class MongoConnectionHandler<TEntity> where TEntity : IDocumentEntity
    {
        public IMongoCollection<TEntity> MongoCollection { get; }

        public MongoConnectionHandler()
        {
            try
            {
                var connectionString =
                    $"{EvenstDispatcherConfiguration.MongoDbSection.ConnectionString}";

                if (!string.IsNullOrEmpty(EvenstDispatcherConfiguration.MongoDbSection.ReplicaSet))
                    connectionString = $"{connectionString}&{EvenstDispatcherConfiguration.MongoDbSection.ReplicaSet}";

                if (!string.IsNullOrEmpty(EvenstDispatcherConfiguration.MongoDbSection.AuthSource))
                    connectionString = $"{connectionString}&{EvenstDispatcherConfiguration.MongoDbSection.AuthSource}";

                var mongoClientSettings = 
                    MongoClientSettings.FromUrl(new MongoUrl(connectionString));
                var mongoClient = new MongoClient(mongoClientSettings);

                var mongoDatabase = mongoClient.GetDatabase("4SolidCqrs");
                
                var typeName = typeof(TEntity).Name.ToLower();
                typeName = typeName.Replace("nosql", "");

                this.MongoCollection = mongoDatabase.GetCollection<TEntity>(typeName + "Collection");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(ex.Message);
            }
        }
    }
}