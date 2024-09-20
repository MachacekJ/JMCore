﻿using MongoDB.Bson.Serialization.Attributes;
// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace ACore.Server.Modules.AuditModule.Storage.Mongo.Models;

internal class AuditMongoValueEntity
{
  // name
  [BsonElement("n")]
  public string Property { get; set; }
  
  // type
  [BsonElement("t")]
  public string DataType { get; set; }
  
  // changed
  [BsonElement("ch")]
  public bool IsChanged { get; set; }
  
  [BsonElement("ov")]
  public string? OldValue { get; set; }
  [BsonElement("nv")]
  public string? NewValue { get; set; }
}