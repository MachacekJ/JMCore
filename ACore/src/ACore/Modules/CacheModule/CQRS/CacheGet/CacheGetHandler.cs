using ACore.Models;
using ACore.Modules.CacheModule.CQRS.Models;

namespace ACore.Modules.CacheModule.CQRS.CacheGet;

public class CacheGetHandler(IJMCache cache) : CacheModuleRequestHandler<CacheModuleGetQuery, JMCacheValue?>
{
    private readonly IJMCache _cache = cache ?? throw new ArgumentException($"{nameof(cache)} is null.");

    public override Task<Result<JMCacheValue?>> Handle(CacheModuleGetQuery request, CancellationToken cancellationToken)
    {
        object? value;
        JMCacheValue? cacheValue = null;
        var result = _cache.TryGetValue(request.Key, out value);
        if (result)
            cacheValue = new JMCacheValue(value);

        return Task.FromResult(Result.Success(cacheValue));
    }
}