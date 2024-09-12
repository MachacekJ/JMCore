using ACore.Models;
using ACore.Modules.MemoryCacheModule.Storages;
using ACore.Services.Cache;
using ACore.Services.Cache.Models;

namespace ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheGet;

public class MemoryMemoryCacheGetHandler(IMemoryCacheStorage cache) : MemoryCacheModuleRequestHandler<MemoryCacheModuleGetQuery, Result<CacheValue?>>
{
    private readonly IMemoryCacheStorage _cache = cache ?? throw new ArgumentException($"{nameof(cache)} is null.");

    public override Task<Result<CacheValue?>> Handle(MemoryCacheModuleGetQuery request, CancellationToken cancellationToken)
    {
        object? value;
        CacheValue? cacheValue = null;
        var result = _cache.TryGetValue(request.Key, out value);
        if (result)
            cacheValue = new CacheValue(value);

        return Task.FromResult(Result.Success(cacheValue));
    }
}