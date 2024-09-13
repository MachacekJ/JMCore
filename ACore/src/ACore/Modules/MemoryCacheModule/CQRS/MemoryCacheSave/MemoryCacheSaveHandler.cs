using ACore.Base.CQRS.Models;
using ACore.Models;
using ACore.Modules.MemoryCacheModule.Storages;

namespace ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheSave;

public class MemoryCacheSaveHandler(IMemoryCacheModuleStorage cacheModule) : MemoryCacheModuleRequestHandler<MemoryCacheModuleSaveCommand, Result<bool>>
{
  private readonly IMemoryCacheModuleStorage _cacheModule = cacheModule ?? throw new ArgumentException($"{nameof(cacheModule)} is null.");

  public override Task<Result<bool>> Handle(MemoryCacheModuleSaveCommand request, CancellationToken cancellationToken)
  {
    _cacheModule.Set(request.Key, request.Value);
    return Task.FromResult(Result.Success(true));
  }
}