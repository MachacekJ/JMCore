namespace ACore.Server.Modules.AuditModule.CQRS.AuditGet.Models;

public class AuditGetQueryColumnDataOut(string columnName, bool isChange, string dataType, object? oldValue, object? newValue)
{
  public bool IsChange => isChange;
  public string ColumnName => columnName;
  public string DataType => dataType;
  public object? OldValue => oldValue;
  public object? NewValue => newValue;
}