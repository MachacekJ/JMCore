using ACore.Server.Modules.AuditModule.UserProvider;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;

namespace ACore.Server.Modules.AuditModule.Models;

public class AuditEntryValueItem(string columnName, object? oldValue, object? newValue)
{
  public Type DataType
  {
    get
    {
      Type? valueDataType = null;
      if (newValue != null)
        valueDataType = newValue.GetType();
      if (oldValue != null)
        valueDataType = oldValue.GetType();
      if (valueDataType == null)
        throw new Exception($"Unknown data type of value. ColumnName: {columnName}");
      return valueDataType;
    }
  }
  public string ColumnName => columnName;
  public object? OldValue => oldValue;
  public object? NewValue => newValue;
}

public class AuditEntryItem
{
  public string TableName { get; }
  public string? SchemaName { get; }
  public List<AuditEntryValueItem> ChangedColumns { get; } = [];
  public EntityState EntityState { get; set; }
  public (string userId, string userName) ByUser { get; }
  public long? PkValue { get; private set; }
  public string? PkValueString { get; set; }

  public AuditEntryItem(string tableName, string? schemaName, object pkValue, IAuditUserProvider auditUserProvider)
  {
    TableName = tableName;
    SchemaName = schemaName;
    ByUser = auditUserProvider.GetUser();
    SetPK(pkValue);
  }

  public void AddEntry(string columnName, object? oldValue, object? newValue)
  {
    ChangedColumns.Add(new AuditEntryValueItem(columnName, oldValue, newValue));
  }

  public void SetPK<TPK>(TPK pkValue)
  {
    if (long.TryParse(pkValue.ToString(), out var pkv))
      PkValue = pkv;
    else
      PkValueString = pkValue.ToString();
  }
}