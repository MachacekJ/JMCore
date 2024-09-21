using ACore.Server.Modules.AuditModule.Models;
using ACore.Server.Storages.Models.PK;
using Microsoft.EntityFrameworkCore;

namespace ACore.Server.Modules.AuditModule.CQRS.AuditGet.Models;

public class AuditGetQueryDataOut<TEntity, TPK>(string tableName)
  where TEntity : PKEntity<TPK>
{
  public string TableName => tableName;
  public string? SchemaName { get; set; }
  public long? PKValue { get; set; }
  public string? PKValueString { get; set; }
  public string? UserId { get; set; }
  public DateTime DateTime { get; set; }
  public AuditStateEnum EntityState { get; set; }
  public AuditGetQueryColumnDataOut[] Columns { get; set; } = [];
  
  public TPK PK { get; set; }

  public static AuditGetQueryDataOut<TEntity, TPK> Create(AuditEntryItem auditEntryItem)
  {
    return new AuditGetQueryDataOut<TEntity, TPK>(auditEntryItem.TableName)
    {
      DateTime = auditEntryItem.Created,
      EntityState = auditEntryItem.EntityState.ToAuditStateEnum(),
      UserId = auditEntryItem.UserId,
      PKValueString = auditEntryItem.PkValueString,
      PKValue = auditEntryItem.PkValue,
      PK = auditEntryItem.GetPK<TPK>() ?? throw new Exception("Primary key is null."),
      SchemaName = auditEntryItem.SchemaName,
      Columns = auditEntryItem.ChangedColumns
        .Select(e => new AuditGetQueryColumnDataOut(e.ColumnName, e.IsChanged, e.DataType, e.OldValue, e.NewValue))
        .ToArray()
    };
  }
}

public static class AuditGetQueryDataOutExtensions
{
  public static AuditGetQueryColumnDataOut? GetColumn<TEntity, TPK>(this AuditGetQueryDataOut<TEntity, TPK> auditValueData, string columnName)
    where TEntity : PKEntity<TPK>
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