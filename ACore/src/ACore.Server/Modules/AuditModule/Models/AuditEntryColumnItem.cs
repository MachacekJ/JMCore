namespace ACore.Server.Modules.AuditModule.Models;

public class AuditEntryColumnItem(string columnName, bool isChanged, string dataType, object? oldValue, object? newValue)
{
  public bool IsChanged => isChanged;
  
  public string DataType =>dataType;
  
  public string ColumnName => columnName;
  public object? OldValue => oldValue;
  public object? NewValue => newValue;
}