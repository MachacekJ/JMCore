using MongoDB.Bson.Serialization.Attributes;

namespace ACore.Server.Modules.AuditModule.Storage.Mongo.Models;

internal class AuditMongoUserEntity
{
  [BsonElement("id")]
  public string Id { get; set; }
  [BsonElement("n")]
  public string Name { get; set; }
}