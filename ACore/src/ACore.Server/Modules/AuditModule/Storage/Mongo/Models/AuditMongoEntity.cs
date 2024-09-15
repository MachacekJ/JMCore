using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ACore.Server.Modules.AuditModule.Storage.Mongo.Models;

internal class AuditMongoEntity
{
  [Key]
  public ObjectId _id { get; set; }

  [BsonElement("oid")]
  public string ObjectId { get; set; } = string.Empty;
  
  [BsonElement("v")]
  public int Version { get; set; }
  
  [BsonElement("c")]
  public List<AuditMongoValueEntity>? Columns { get; set; }
  
  [BsonElement("t")]
  public DateTime Created { get; set; }
  
  [BsonElement("s")]
  public EntityState EntityState { get; set; }
  
  [BsonElement("u")]
  public AuditMongoUserEntity? User { get; set; }
}