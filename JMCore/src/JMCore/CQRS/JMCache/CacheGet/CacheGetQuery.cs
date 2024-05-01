using JMCore.Services.JMCache;

namespace JMCore.CQRS.JMCache.CacheGet;

/// <summary>
/// Returns cache value for key.
/// null = cache value is not saved in cache.
/// <see cref="JMCacheValue"/>.<see cref="JMCacheValue.Value"/> = null, cache value is null, but is in cache.
/// </summary>
public class CacheGetQuery(JMCacheKey key) : ICacheRequest<JMCacheValue?>
{
    public JMCacheKey Key { get; } = key;
}