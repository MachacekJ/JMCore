using ACore.Base.CQRS.Models.Results;
using ACore.Modules.MemoryCacheModule.Storages;

namespace ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheRemove;

public class MemoryCacheRemoveCategoryHandler(IMemoryCacheModuleStorage cacheModule) : MemoryCacheModuleRequestHandler<MemoryCacheModuleRemoveCategoryCommand, Result<bool>>
{
  private readonly IMemoryCacheModuleStorage _cacheModule = cacheModule ?? throw new ArgumentException($"{nameof(cacheModule)} is null.");

  public override Task<Result<bool>> Handle(MemoryCacheModuleRemoveCategoryCommand request, CancellationToken cancellationToken)
  {
    if (request.MainCategory != null)
      _cacheModule.RemoveCategory(request.MainCategory, request.SubCategory);

    return Task.FromResult(Result.Success(true));
  }
}