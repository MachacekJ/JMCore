namespace ACore.Modules.CacheModule.CQRS.Models;

public class JMCacheValue(object? cacheValue)
{
    public object? CacheValue { get; } = cacheValue;
}