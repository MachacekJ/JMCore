using ACore.Base.CQRS.Results;
using ACore.Modules.MemoryCacheModule.Storages;

namespace ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheRemove;

public class MemoryCacheRemoveKeyHandler(IMemoryCacheModuleStorage cacheModule) : MemoryCacheModuleRequestHandler<MemoryCacheModuleRemoveKeyCommand, Result<bool>>
{
  private readonly IMemoryCacheModuleStorage _cacheModule = cacheModule ?? throw new ArgumentException($"{nameof(cacheModule)} is null.");

  public override Task<Result<bool>> Handle(MemoryCacheModuleRemoveKeyCommand request, CancellationToken cancellationToken)
  {
    if (request.Key != null)
      _cacheModule.Remove(request.Key);
    
    return Task.FromResult(Result.Success(true));
  }
}