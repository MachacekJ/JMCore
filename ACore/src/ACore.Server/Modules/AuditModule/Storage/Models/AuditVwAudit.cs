using Microsoft.EntityFrameworkCore;

namespace ACore.Server.Modules.AuditModule.Storage.Models;

public class AuditVwAuditEntity
{
  public long Id { get; set; }
  public long AuditId { get; set; }
  public string TableName { get; set; } = null!;
  public string? SchemaName { get; set; }
  public long? PKValue { get; set; }
  public string? PKValueString { get; set; }

  public string? UserName { get; set; }
  public DateTime DateTime { get; set; }
  public EntityState EntityState { get; set; }
  public string ColumnName { get; set; } = null!;

  public string? OldValueString { get; set; }
  public string? NewValueString { get; set; }
  public int? OldValueInt { get; set; }
  public int? NewValueInt { get; set; }

  public long? OldValueLong { get; set; }
  public long? NewValueLong { get; set; }

  public bool? OldValueBool { get; set; }
  public bool? NewValueBool { get; set; }
  public Guid? OldValueGuid { get; set; }
  public Guid? NewValueGuid { get; set; }
}