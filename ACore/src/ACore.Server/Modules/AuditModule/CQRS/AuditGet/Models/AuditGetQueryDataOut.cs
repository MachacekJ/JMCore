using ACore.Server.Modules.AuditModule.Models;
using Microsoft.EntityFrameworkCore;

namespace ACore.Server.Modules.AuditModule.CQRS.AuditGet.Models;

public record AuditGetQueryDataOut<TPK>(string TableName, string? SchemaName , TPK PrimaryKey, string UserId, DateTime Created, AuditInfoStateEnum State, AuditGetQueryColumnDataOut[] Columns)
{
  public static AuditGetQueryDataOut<TPK> Create(AuditInfoItem saveInfoItem)
  {
    return new AuditGetQueryDataOut<TPK>(
      saveInfoItem.TableName,
      saveInfoItem.SchemaName,
      saveInfoItem.GetPK<TPK>() ?? throw new Exception("Primary key is null."),
    saveInfoItem.UserId,
    saveInfoItem.Created,
    saveInfoItem.EntityState.ToAuditStateEnum(),
    saveInfoItem.Columns
      .Select(e => new AuditGetQueryColumnDataOut(e.PropName, e.ColumnName, e.IsChanged, e.DataType, e.OldValue, e.NewValue))
      .ToArray()
      );
  }
}

public static class AuditGetQueryDataOutExtensions
{
  public static AuditGetQueryColumnDataOut? GetColumn<TPK>(this AuditGetQueryDataOut<TPK> auditValueData, string columnName)
  {
    return auditValueData.Columns.SingleOrDefault(e => e.ColumnName == columnName);
  }

  public static AuditInfoStateEnum ToAuditStateEnum(this EntityState entityState)
  {
    return entityState switch
    {
      EntityState.Added => AuditInfoStateEnum.Added,
      EntityState.Deleted => AuditInfoStateEnum.Deleted,
      EntityState.Modified => AuditInfoStateEnum.Modified,
      _ => throw new ArgumentOutOfRangeException(nameof(entityState), entityState, null)
    };
  }
}