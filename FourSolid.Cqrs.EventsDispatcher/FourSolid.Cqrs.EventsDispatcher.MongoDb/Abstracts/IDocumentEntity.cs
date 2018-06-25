using MongoDB.Bson.Serialization.Attributes;

namespace FourSolid.Cqrs.EventsDispatcher.MongoDb.Abstracts
{
    public interface IDocumentEntity
    {
        [BsonId]
        string Id { get; set; }
    }
}