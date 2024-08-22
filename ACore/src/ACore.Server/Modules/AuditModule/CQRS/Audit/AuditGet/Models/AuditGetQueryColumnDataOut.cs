namespace ACore.Server.Modules.AuditModule.CQRS.Audit.AuditGet.Models;

public class AuditGetQueryColumnDataOut(string columnName, Type dataType, object? oldValue, object? newValue)
{
  public string ColumnName { get; } = columnName;
  public string? DataType { get; } = dataType.FullName;
  public object? OldValue { get; } = oldValue;
  public object? NewValue { get; } = newValue;
}