namespace ACore.Services.Cache.Models;

public class CacheValue(object? objectValue)
{
    public object? ObjectValue { get; } = objectValue;
}