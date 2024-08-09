namespace ACore.Server.Modules.AuditModule.Models;

public record AuditInfoColumnItem(string PropName, string ColumnName, string DataType, bool IsChanged, object? OldValue, object? NewValue);
