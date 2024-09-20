namespace ACore.Server.Modules.AuditModule.Models;

public class AuditEntryColumnItem(string columnName, bool isChanged, object? oldValue, object? newValue)
{
  public bool IsChanged => isChanged;
  
  public string DataType
  {
    get
    {
      string? valueDataType = null;
      if (newValue != null)
        valueDataType = newValue.GetType().FullName;
      if (oldValue != null)
        valueDataType = oldValue.GetType().FullName;
      
      if (valueDataType == null)
        throw new Exception($"Unknown data type of value. ColumnName: {columnName}");
      return valueDataType;
    }
  }
  public string ColumnName => columnName;
  public object? OldValue => oldValue;
  public object? NewValue => newValue;
}