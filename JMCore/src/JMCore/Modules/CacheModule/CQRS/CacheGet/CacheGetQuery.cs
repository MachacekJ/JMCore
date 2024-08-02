using JMCore.Modules.CacheModule.CQRS.Models;

namespace JMCore.Modules.CacheModule.CQRS.CacheGet;

/// <summary>
/// Returns cache value for key.
/// null = cache value is not saved in cache.
/// <see cref="JMCacheValue"/>.<see cref="JMCacheValue.Value"/> = null, cache value is null, but is in cache.
/// </summary>
public class CacheModuleGetQuery(JMCacheKey key) : CacheModuleRequest<JMCacheValue?>
{
    public JMCacheKey Key { get; } = key;
}