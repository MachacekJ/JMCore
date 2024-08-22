using System.ComponentModel.DataAnnotations;

// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace ACore.Server.Modules.AuditModule.Storage.SQL.Models;

internal class AuditTableEntity
{
  [Key]
  public int Id { get; set; }
  
  [MaxLength(255)]
  public string TableName { get; set; } = null!;
  
  [MaxLength(255)]
  public string? SchemaName { get; set; }
}