using ACore.Models;
using ACore.Services.Cache.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheSave;

public class MemoryCacheModuleSaveCommand(CacheKey key, object value) : MemoryCacheModuleRequest<Result<bool>>
{
    public CacheKey Key { get; } = key;
    public object Value { get; } = value;
}