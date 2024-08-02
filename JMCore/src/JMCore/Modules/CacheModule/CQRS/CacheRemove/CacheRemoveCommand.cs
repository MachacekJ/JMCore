using JMCore.Modules.CacheModule.CQRS.Models;

namespace JMCore.Modules.CacheModule.CQRS.CacheRemove;

public class CacheModuleRemoveCommand : CacheModuleRequest<bool>
{
    public JMCacheKey? Key { get; }

    public string? KeyPrefix { get; }

    public int? Category { get; }

    public CacheModuleRemoveCommand(JMCacheKey key)
    {
        Key = key;
    }

    /// <summary>
    /// !!! Please use const eg. JMCacheCategory.Localization <see cref="JMCacheCategory"/>.
    /// </summary>
    public CacheModuleRemoveCommand(int category, string? keyPrefix = null)
    {
        Category = category;
        KeyPrefix = keyPrefix;
    }
}