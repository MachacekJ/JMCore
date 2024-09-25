using ACore.Base.Cache;
using ACore.Base.CQRS.Models.Results;

namespace ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheSave;

public class MemoryCacheModuleSaveCommand(CacheKey key, object value) : MemoryCacheModuleRequest<Result>
{
    public CacheKey Key { get; } = key;
    public object Value { get; } = value;
}