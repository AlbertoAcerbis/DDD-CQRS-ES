using MongoDB.Bson.Serialization.Attributes;

namespace FourSolid.Cqrs.OrdiniClienti.ReadModel.MongoDb.Abstracts
{
    public interface IDocumentEntity
    {
        [BsonId]
        string Id { get; set; }
    }
}