using ACore.Models;

namespace ACore.Modules.CacheModule.CQRS.CacheSave;

public class CacheSaveHandler(IJMCache cache) : CacheModuleRequestHandler<CacheModuleSaveCommand, bool>
{
    private readonly IJMCache _cache = cache ?? throw new ArgumentException($"{nameof(cache)} is null.");

    public override Task<Result<bool>> Handle(CacheModuleSaveCommand request, CancellationToken cancellationToken)
    {
        if (request.Options != null)
            _cache.Set(request.Key, request.Value, request.Options);
        else if (request.ExpirationToken != null)
            _cache.Set(request.Key, request.Value, request.ExpirationToken);
        else if (request.AbsoluteExpiration != null)
            _cache.Set(request.Key, request.Value, request.AbsoluteExpiration.Value);
        else if (request.AbsoluteExpirationRelativeToNow != null)
            _cache.Set(request.Key, request.Value, request.AbsoluteExpirationRelativeToNow.Value);
        else
            _cache.Set(request.Key, request.Value);
        return Task.FromResult(Result.Success(true));
    }
}