using ACore.Models;
using MediatR;

namespace ACore.Modules.CacheModule.CQRS.CacheRemove;

public class CacheRemoveHandler(IJMCache cache) : CacheModuleRequestHandler<CacheModuleRemoveCommand, Result<bool>>
{
  private readonly IJMCache _cache = cache ?? throw new ArgumentException($"{nameof(cache)} is null.");

  public override Task<Result<bool>> Handle(CacheModuleRemoveCommand request, CancellationToken cancellationToken)
  {
    if (request.Key != null)
      _cache.Remove(request.Key);

    if (request.Category != null)
      _cache.RemoveCategory(request.Category.Value, request.KeyPrefix);

    return Task.FromResult(Result.Success(true));
  }
}