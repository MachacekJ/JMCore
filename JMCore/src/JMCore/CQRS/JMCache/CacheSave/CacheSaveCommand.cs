using JMCore.Services.JMCache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace JMCore.CQRS.JMCache.CacheSave;

public class CacheSaveCommand(JMCacheKey key, object value) : ICacheRequest<bool>
{
    public JMCacheKey Key { get; } = key;
    public object Value { get; } = value;
    public MemoryCacheEntryOptions? Options { get; }
    public IChangeToken? ExpirationToken { get; }
    public DateTimeOffset? AbsoluteExpiration { get; }

    public TimeSpan? AbsoluteExpirationRelativeToNow { get; }

    public CacheSaveCommand(JMCacheKey key, object value, MemoryCacheEntryOptions options) : this(key, value)
    {
        Options = options;
    }

    public CacheSaveCommand(JMCacheKey key, object value, IChangeToken expirationToken) : this(key, value)
    {
        ExpirationToken = expirationToken;
    }

    public CacheSaveCommand(JMCacheKey key, object value, DateTimeOffset absoluteExpiration) : this(key, value)
    {
        AbsoluteExpiration = absoluteExpiration;
    }

    public CacheSaveCommand(JMCacheKey key, object value, TimeSpan absoluteExpirationRelativeToNow) : this(key, value)
    {
        AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow;
    }
}