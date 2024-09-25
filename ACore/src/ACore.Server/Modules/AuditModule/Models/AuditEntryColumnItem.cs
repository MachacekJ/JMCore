namespace ACore.Server.Modules.AuditModule.Models;

public class AuditEntryColumnItem(string columnName, bool isChanged, string dataType, object? oldValue, object? newValue)
{
  public bool IsChanged { get; set; } = isChanged;
  
  public string DataType =>dataType;
  
  public string ColumnName => columnName;
  public object? OldValue { get; set; } = oldValue;

  public object? NewValue { get; set; } = newValue;
}