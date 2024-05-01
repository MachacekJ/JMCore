using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JMCore.Server.MongoStorage.BasicModule.Models;

public abstract class BaseCollection
{
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string Id { get; set; }
}