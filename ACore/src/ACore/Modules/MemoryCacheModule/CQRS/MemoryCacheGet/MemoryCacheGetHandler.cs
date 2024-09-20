using ACore.Base.Cache;
using ACore.Base.CQRS.Models;
using ACore.Base.CQRS.Models.Results;
using ACore.Models;
using ACore.Modules.MemoryCacheModule.Storages;

namespace ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheGet;

public class MemoryMemoryCacheGetHandler(IMemoryCacheModuleStorage cacheModule) : MemoryCacheModuleRequestHandler<MemoryCacheModuleGetQuery, Result<CacheValue?>>
{
    private readonly IMemoryCacheModuleStorage _cacheModule = cacheModule ?? throw new ArgumentException($"{nameof(cacheModule)} is null.");

    public override Task<Result<CacheValue?>> Handle(MemoryCacheModuleGetQuery request, CancellationToken cancellationToken)
    {
        object? value;
        CacheValue? cacheValue = null;
        var result = _cacheModule.TryGetValue(request.Key, out value);
        if (result)
            cacheValue = new CacheValue(value);

        return Task.FromResult(Result.Success(cacheValue));
    }
}