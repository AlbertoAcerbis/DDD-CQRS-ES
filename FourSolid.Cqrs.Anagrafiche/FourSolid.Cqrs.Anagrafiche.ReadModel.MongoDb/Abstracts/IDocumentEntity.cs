using MongoDB.Bson.Serialization.Attributes;

namespace FourSolid.Cqrs.Anagrafiche.ReadModel.MongoDb.Abstracts
{
    public interface IDocumentEntity
    {
        [BsonId]
        string Id { get; set; }
    }
}