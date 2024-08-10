using MongoDB.Bson.Serialization.Attributes;

namespace ACore.Server.MongoStorage.AuditModule.Models;

public class AuditMongoValueEntity
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