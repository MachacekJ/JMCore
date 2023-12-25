using JMCore.Services.JMCache;
using MediatR;

namespace JMCore.CQRS.JMCache.CacheGet;

public class CacheGetHandler(IJMCache cache) : IRequestHandler<CacheGetQuery, JMCacheValue?>
{
    private readonly IJMCache _cache = cache ?? throw new ArgumentException($"{nameof(cache)} is null.");

    public Task<JMCacheValue?> Handle(CacheGetQuery request, CancellationToken cancellationToken)
    {
        object? value;
        JMCacheValue? cacheValue = null;
        var result = _cache.TryGetValue(request.Key, out value);
        if (result)
            cacheValue = new JMCacheValue(value);

        return Task.FromResult(cacheValue);
    }
}