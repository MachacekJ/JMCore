using ACore.Modules.CacheModule.CQRS.Models;
using MediatR;

namespace ACore.Modules.CacheModule.CQRS.CacheGet;

public class CacheGetHandler(IJMCache cache) : IRequestHandler<CacheModuleGetQuery, JMCacheValue?>
{
    private readonly IJMCache _cache = cache ?? throw new ArgumentException($"{nameof(cache)} is null.");

    public Task<JMCacheValue?> Handle(CacheModuleGetQuery request, CancellationToken cancellationToken)
    {
        object? value;
        JMCacheValue? cacheValue = null;
        var result = _cache.TryGetValue(request.Key, out value);
        if (result)
            cacheValue = new JMCacheValue(value);

        return Task.FromResult(cacheValue);
    }
}