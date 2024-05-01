using JMCore.Services.JMCache;

namespace JMCore.CQRS.JMCache.CacheRemove;

public class CacheRemoveCommand : ICacheRequest<bool>
{
    public JMCacheKey? Key { get; }

    public string? KeyPrefix { get; }

    public int? Category { get; }

    public CacheRemoveCommand(JMCacheKey key)
    {
        Key = key;
    }

    /// <summary>
    /// !!! Please use const eg. JMCacheCategory.Localization <see cref="JMCacheCategory"/>.
    /// </summary>
    public CacheRemoveCommand(int category, string? keyPrefix = null)
    {
        Category = category;
        KeyPrefix = keyPrefix;
    }
}