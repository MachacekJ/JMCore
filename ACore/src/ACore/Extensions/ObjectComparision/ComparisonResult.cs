namespace ACore.Extensions.ObjectComparision;
public record ComparisonResult(string Name, Type Type, bool IsChange, object? LeftValue, object? RightValue);