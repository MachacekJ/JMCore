using MongoDB.Bson.Serialization.Attributes;

namespace ACore.Server.Modules.AuditModule.Storage.Mongo.Models;

internal class AuditMongoValueEntity
{
  [BsonElement("n")]
  public string Property { get; set; }
  
  [BsonElement("t")]
  public string DataType { get; set; }

  [BsonElement("ov")]
  public string? OldValue { get; set; }
  [BsonElement("nv")]
  public string? NewValue { get; set; }
}