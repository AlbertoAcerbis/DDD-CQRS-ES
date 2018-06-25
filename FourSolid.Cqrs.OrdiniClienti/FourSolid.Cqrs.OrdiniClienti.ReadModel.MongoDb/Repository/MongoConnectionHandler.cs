using System;
using FourSolid.Cqrs.OrdiniClienti.ReadModel.MongoDb.Abstracts;
using FourSolid.Cqrs.OrdiniClienti.Shared.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FourSolid.Cqrs.OrdiniClienti.ReadModel.MongoDb.Repository
{
    public class MongoConnectionHandler<TEntity> where TEntity : IDocumentEntity
    {
        public IMongoCollection<TEntity> MongoCollection { get; }

        public MongoConnectionHandler(IOptions<FourSettings> authOptions)
        {
           var authSettings = authOptions.Value;

            try
            {
                var mongoClientSettings = 
                    MongoClientSettings.FromUrl(new MongoUrl(authSettings.MongoDbParameters.ConnectionString));
                var mongoClient = new MongoClient(mongoClientSettings);

                var mongoDatabase = mongoClient.GetDatabase("CQRSOrdiniClienti");
                
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