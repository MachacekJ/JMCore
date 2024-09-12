using ACore.Models;
using ACore.Modules.MemoryCacheModule.Storages;

namespace ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheSave;

public class MemoryCacheSaveHandler(IMemoryCacheStorage cache) : MemoryCacheModuleRequestHandler<MemoryCacheModuleSaveCommand, Result<bool>>
{
  private readonly IMemoryCacheStorage _cache = cache ?? throw new ArgumentException($"{nameof(cache)} is null.");

  public override Task<Result<bool>> Handle(MemoryCacheModuleSaveCommand request, CancellationToken cancellationToken)
  {
    _cache.Set(request.Key, request.Value);
    return Task.FromResult(Result.Success(true));
  }
}