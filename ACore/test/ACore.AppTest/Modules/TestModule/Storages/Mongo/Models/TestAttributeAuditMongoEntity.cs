using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACore.AppTest.Modules.TestModule.Storages.PG;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Storages;
using ACore.Server.Storages.Mongo;
using MongoDB.Bson.Serialization.Attributes;

// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ACore.AppTest.Modules.TestModule.Storages.Mongo.Models;

/// <summary>
/// Sample: How to use <see cref="AuditableAttribute"/> for entity.
/// This entity is used for more storages like MongoDb, Postgres etc.
/// Column name <see cref="ColumnAttribute"/> for saving in storage is defined e.g. <see cref="PGTestStorageDbNames"/>.
/// </summary>
[Auditable]
public class TestAttributeAuditMongoEntity: MongoStorageEntity
{

  [MaxLength(50)]
  [BsonElement("name")]
  public string Name { get; set; } = string.Empty;

  [NotAuditable]
  [MaxLength(50)]
  [BsonElement("notAuditableColumn")]
  public string NotAuditableColumn { get; set; } = string.Empty;
  
  [BsonElement("created")]
  public DateTime Created { get; set; }
}