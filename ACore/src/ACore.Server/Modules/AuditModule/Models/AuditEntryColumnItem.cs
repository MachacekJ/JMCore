namespace ACore.Server.Modules.AuditModule.Models;

public class AuditEntryColumnItem(string columnName, object? oldValue, object? newValue)
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