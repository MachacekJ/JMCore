using ACore.Modules.CacheModule.CQRS.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace ACore.Modules.CacheModule.CQRS.CacheSave;

public class CacheModuleSaveCommand(JMCacheKey key, object value) : CacheModuleRequest<bool>
{
    public JMCacheKey Key { get; } = key;
    public object Value { get; } = value;
    public MemoryCacheEntryOptions? Options { get; }
    public IChangeToken? ExpirationToken { get; }
    public DateTimeOffset? AbsoluteExpiration { get; }

    public TimeSpan? AbsoluteExpirationRelativeToNow { get; }

    public CacheModuleSaveCommand(JMCacheKey key, object value, MemoryCacheEntryOptions options) : this(key, value)
    {
        Options = options;
    }

    public CacheModuleSaveCommand(JMCacheKey key, object value, IChangeToken expirationToken) : this(key, value)
    {
        ExpirationToken = expirationToken;
    }

    public CacheModuleSaveCommand(JMCacheKey key, object value, DateTimeOffset absoluteExpiration) : this(key, value)
    {
        AbsoluteExpiration = absoluteExpiration;
    }

    public CacheModuleSaveCommand(JMCacheKey key, object value, TimeSpan absoluteExpirationRelativeToNow) : this(key, value)
    {
        AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow;
    }
}