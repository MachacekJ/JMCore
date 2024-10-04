namespace ACore.Extensions;
public record ComparisonResult(string Name, Type Type, bool IsChange, object? LeftValue, object? RightValue);