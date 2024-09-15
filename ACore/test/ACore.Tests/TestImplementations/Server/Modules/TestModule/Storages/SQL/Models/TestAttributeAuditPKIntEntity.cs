using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACore.Extensions;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Storages.Models.PK;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAttributeAudit.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.Mongo.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.PG;

// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

/// <summary>
/// Sample: How to use <see cref="AuditableAttribute"/> for entity.
/// This entity is used for more storages like MongoDb, Postgres etc.
/// Column name <see cref="ColumnAttribute"/> for saving in storage is defined e.g. <see cref="DefaultNames"/>.
/// </summary>
[Auditable(1)]
internal class TestAttributeAuditPKIntEntity: PKIntEntity
{
  [MaxLength(50)]
  public string Name { get; set; } = string.Empty;

  [NotAuditable]
  [MaxLength(50)]
  public string NotAuditableColumn { get; set; } = string.Empty;
  
  public DateTime Created { get; set; }
  
  public static TestAttributeAuditPKIntEntity Create<T>(TestAttributeAuditData<T> data)
  {
    var en = new TestAttributeAuditPKIntEntity();
    en.CopyPropertiesFrom(data);
    return en;
  }
}