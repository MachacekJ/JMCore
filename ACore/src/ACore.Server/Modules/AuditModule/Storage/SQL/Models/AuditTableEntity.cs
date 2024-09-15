using System.ComponentModel.DataAnnotations;
using ACore.Server.Storages.Models.PK;

// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace ACore.Server.Modules.AuditModule.Storage.SQL.Models;

internal class AuditTableEntity : PKIntEntity
{
  [MaxLength(255)]
  [Required]
  public string TableName { get; set; }

  [MaxLength(255)]
  public string? SchemaName { get; set; }

  public int Version { get; set; }
}