using MongoDB.Bson.Serialization.Attributes;

namespace ACore.Server.MongoStorage.AuditModule.Models;

public class AuditMongoValueEntity
{
  [BsonElement("p")]
  public string Property { get; set; }

  [BsonElement("v")]
  public string Value { get; set; }
}