using MongoDB.Bson.Serialization.Attributes;

namespace JMCore.Server.MongoStorage.AuditModule.Models;

public class AuditMongoUserEntity
{
  [BsonElement("id")]
  public string Id { get; set; }
  [BsonElement("n")]
  public string Name { get; set; }
}