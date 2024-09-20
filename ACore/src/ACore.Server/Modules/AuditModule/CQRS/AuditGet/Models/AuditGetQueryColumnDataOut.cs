namespace ACore.Server.Modules.AuditModule.CQRS.AuditGet.Models;

public class AuditGetQueryColumnDataOut(string columnName, bool isChange, Type dataType, object? oldValue, object? newValue)
{
  public bool IsChange => isChange;
  public string ColumnName => columnName;
  public string DataType => dataType.FullName ?? throw new ArgumentNullException($"Null value '{nameof(dataType.FullName)}' for data type '{dataType.Name}'");
  public object? OldValue => oldValue;
  public object? NewValue => newValue;
}