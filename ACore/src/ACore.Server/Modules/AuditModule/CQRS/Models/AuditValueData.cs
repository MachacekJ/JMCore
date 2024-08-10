using Microsoft.EntityFrameworkCore;

namespace ACore.Server.Modules.AuditModule.CQRS.Models;

public class AuditValueData
{
  // public long Id { get; set; }
  // public long AuditId { get; set; }
  public string TableName { get; set; } = null!;
  public string? SchemaName { get; set; }
  public long? PKValue { get; set; }
  public string? PKValueString { get; set; }

  public string? UserName { get; set; }
  public DateTime DateTime { get; set; }
  public AuditStateEnum EntityState { get; set; }
  public AuditValueColumnData[] Columns { get; set; } = [];
}

public class AuditValueColumnData(string columnName, string dataType, object? oldValue, object? newValue)
{
  public string ColumnName { get; } = columnName;
  public string DataType { get; } = dataType;
  public object? OldValue { get; } = oldValue;
  public object? NewValue { get; } = newValue;
}

public static class AuditValueDataExtensions
{
  public static AuditValueColumnData? GetColumn(this AuditValueData auditValueData, string columnName)
  {
    return auditValueData.Columns.SingleOrDefault(e => e.ColumnName == columnName);
  }

  public static AuditStateEnum ToAuditStateEnum(this EntityState entityState)
  {
    return entityState switch
    {
      EntityState.Added => AuditStateEnum.Added,
      EntityState.Deleted => AuditStateEnum.Deleted,
      EntityState.Detached => AuditStateEnum.Detached,
      EntityState.Modified => AuditStateEnum.Modified,
      EntityState.Unchanged => AuditStateEnum.Unchanged,
      _ => throw new ArgumentOutOfRangeException(nameof(entityState), entityState, null)
    };
  }
}