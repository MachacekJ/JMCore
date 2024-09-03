using ACore.Modules.CacheModule.CQRS.Models;

namespace ACore.Modules.CacheModule.CQRS.CacheGet;

/// <summary>
/// Returns cache value for key.
/// null = cache value is not saved in cache.
/// <see cref="JMCacheValue"/>.<see cref="JMCacheValue.CacheValue"/> = null, cache value is null, but is in cache.
/// </summary>
public class CacheModuleGetQuery(JMCacheKey key) : CacheModuleRequest<JMCacheValue?>
{
    public JMCacheKey Key { get; } = key;
}