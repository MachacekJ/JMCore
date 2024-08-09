using System.ComponentModel.DataAnnotations;
using ACore.Server.Storages.Definitions.Models.PK;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


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