using ACore.Models;
using ACore.Modules.MemoryCacheModule.Storages;

namespace ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheRemove;

public class MemoryCacheRemoveKeyHandler(IMemoryCacheStorage cache) : MemoryCacheModuleRequestHandler<MemoryCacheModuleRemoveKeyCommand, Result<bool>>
{
  private readonly IMemoryCacheStorage _cache = cache ?? throw new ArgumentException($"{nameof(cache)} is null.");

  public override Task<Result<bool>> Handle(MemoryCacheModuleRemoveKeyCommand request, CancellationToken cancellationToken)
  {
    if (request.Key != null)
      _cache.Remove(request.Key);
    
    return Task.FromResult(Result.Success(true));
  }
}