using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson.Serialization.Attributes;

namespace ACore.Server.MongoStorage.AuditModule.Models;

public class AuditMongoEntity
{
  [Key]
  public string Id { get; set; } = Guid.NewGuid().ToString();

  [BsonElement("oid")]
  public string ObjectId { get; set; } = null!;
  
  [BsonElement("c")]
  public List<AuditMongoValueEntity>? Columns { get; set; }
  
  [BsonElement("t")]
  public DateTime Time { get; set; }
  
  [BsonElement("s")]
  public EntityState EntityState { get; set; }
  
  [BsonElement("u")]
  public AuditMongoUserEntity? User { get; set; }
}