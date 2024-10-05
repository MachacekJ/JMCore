using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Storages.Definitions.Models.PK;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Models;
using MongoDB.Bson.Serialization.Attributes;

// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.Mongo.Models;

/// <summary>
/// Sample: How to use <see cref="AuditableAttribute"/> for entity.
/// This entity is used for more storages like MongoDb, Postgres etc.
/// Column name <see cref="ColumnAttribute"/> for saving in storage is defined e.g. <see cref="SQL.PG.DefaultNames"/>.
/// </summary>
[Auditable(1)]
public class TestAuditEntity: PKMongoEntity
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
  
  [MaxLength(50)]
  [BsonElement("nullValue")]
  public string? NullValue { get; set; }
  [MaxLength(50)]
  [BsonElement("nullValue2")]
  public string? NullValue2 { get; set; }
  [MaxLength(50)]
  [BsonElement("nullValue3")]
  public string? NullValue3 { get; set; }
  
  public static TestAuditEntity Create<T>(TestAuditData<T> data)
    => ToEntity<TestAuditEntity>(data);
}