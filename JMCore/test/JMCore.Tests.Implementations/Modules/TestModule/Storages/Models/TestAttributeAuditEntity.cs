using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JMCore.Server.Modules.AuditModule.Configuration;
using JMCore.Tests.Implementations.Modules.TestModule.Storages.PG;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace JMCore.Tests.Implementations.Modules.TestModule.Storages.Models;

/// <summary>
/// Sample: How to use <see cref="AuditableAttribute"/> for entity.
/// This entity is used for more storages like MongoDb, Postgres etc.
/// Column name <see cref="ColumnAttribute"/> for saving in storage is defined e.g. <see cref="TestPGEfDbNames"/>.
/// </summary>
[Auditable]
public class TestAttributeAuditEntity
{
  /// <summary>
  /// For the purposes of mongodb.
  /// </summary>
  [MaxLength(36)]
  public string UId { get; set; } = Guid.NewGuid().ToString();
  
  /// <summary>
  /// For the purposes of Postgres.
  /// </summary>
  public int Id { get; set; }
  
  [MaxLength(50)]
  public string Name { get; set; } = null!;

  [NotAuditable]
  [MaxLength(50)]
  public string NotAuditableColumn { get; set; } = null!;
  
  public DateTime Created { get; set; }
}