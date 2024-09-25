using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength
// ReSharper disable PropertyCanBeMadeInitOnly.Global
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace ACore.Server.Modules.AuditModule.Storage.Mongo.Models;

internal class AuditMongoEntity
{
  [Key]
  [BsonElement("_id")]
  public ObjectId Id { get; set; }

  [BsonElement("oid")]
  public string ObjectId { get; set; }
  
  [BsonElement("v")]
  public int Version { get; set; }
  
  [BsonElement("c")]
  public List<AuditMongoValueEntity>? Columns { get; set; }
  
  [BsonElement("t")]
  public DateTime Created { get; set; }
  
  [BsonElement("s")]
  public EntityState EntityState { get; set; }
  
  [BsonElement("u")]
  public AuditMongoUserEntity User { get; set; }
}