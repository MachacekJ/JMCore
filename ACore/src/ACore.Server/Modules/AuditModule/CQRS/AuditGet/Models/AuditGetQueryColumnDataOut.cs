namespace ACore.Server.Modules.AuditModule.CQRS.AuditGet.Models;

public record AuditGetQueryColumnDataOut(string PronName, string ColumnName, bool IsChange, string DataType, object? OldValue, object? NewValue);
