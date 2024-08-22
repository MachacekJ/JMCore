using ACore.Server.Modules.AuditModule.Models;
using Microsoft.EntityFrameworkCore;

namespace ACore.Server.Modules.AuditModule.CQRS.Audit.AuditGet.Models;

public class AuditGetQueryDataOut(string tableName)
{
  public string TableName => tableName;
  public string? SchemaName { get; set; }
  public long? PKValue { get; set; }
  public string? PKValueString { get; set; }
  public (string,string)? UserName { get; set; }
  public DateTime DateTime { get; set; }
  public AuditStateEnum EntityState { get; set; }
  public AuditGetQueryColumnDataOut[] Columns { get; set; } = [];
  public static AuditGetQueryDataOut Create(AuditEntryItem auditEntryItem)
  {
    return new AuditGetQueryDataOut(auditEntryItem.TableName)
    {
      DateTime = auditEntryItem.Created,
      EntityState = auditEntryItem.EntityState.ToAuditStateEnum(),
      UserName = auditEntryItem.ByUser,
      PKValueString = auditEntryItem.PkValueString,
      PKValue = auditEntryItem.PkValue,
      SchemaName = auditEntryItem.SchemaName,
      Columns = auditEntryItem.ChangedColumns
        .Select(e => new AuditGetQueryColumnDataOut(e.ColumnName,e.DataType, e.OldValue, e.NewValue))
        .ToArray()
    };
  }
}

public static class AuditGetQueryDataOutExtensions
{
  public static AuditGetQueryColumnDataOut? GetColumn(this AuditGetQueryDataOut auditValueData, string columnName)
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