using JMCore.Services.JMCache;
using MediatR;

namespace JMCore.CQRS.JMCache.CacheRemove;

public class CacheRemoveHandler(IJMCache cache) : IRequestHandler<CacheRemoveCommand, bool>
{
    private readonly IJMCache _cache = cache ?? throw new ArgumentException($"{nameof(cache)} is null.");

    public Task<bool> Handle(CacheRemoveCommand request, CancellationToken cancellationToken)
    {
        if (request.Key != null)
            _cache.Remove(request.Key);

        if (request.Category != null)
            _cache.RemoveCategory(request.Category.Value, request.KeyPrefix);
        
        return Task.FromResult(true);
    }
}