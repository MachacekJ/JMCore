namespace ACore.Modules.CacheModule.CQRS.Models;

public class JMCacheValue(object? value)
{
    public object? Value { get; } = value;
}