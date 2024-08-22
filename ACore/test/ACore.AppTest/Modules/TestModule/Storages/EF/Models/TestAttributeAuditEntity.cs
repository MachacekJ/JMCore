using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACore.AppTest.Modules.TestModule.Storages.PG;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Storages.Models;

// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ACore.AppTest.Modules.TestModule.Storages.EF.Models;

/// <summary>
/// Sample: How to use <see cref="AuditableAttribute"/> for entity.
/// This entity is used for more storages like MongoDb, Postgres etc.
/// Column name <see cref="ColumnAttribute"/> for saving in storage is defined e.g. <see cref="PGTestStorageDbNames"/>.
/// </summary>
[Auditable]
internal class TestAttributeAuditEntity: IntStorageEntity
{
  [MaxLength(50)]
  public string Name { get; set; } = string.Empty;

  [NotAuditable]
  [MaxLength(50)]
  public string NotAuditableColumn { get; set; } = string.Empty;
  
  public DateTime Created { get; set; }
}