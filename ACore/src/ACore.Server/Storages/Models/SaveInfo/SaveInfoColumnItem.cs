namespace ACore.Server.Storages.Models.SaveInfo;

public record SaveInfoColumnItem(bool IsAuditable, string PropName, string ColumnName, string DataType, bool IsChanged, object? OldValue, object? NewValue);