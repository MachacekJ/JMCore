using ACore.Models;
using ACore.Modules.MemoryCacheModule.Storages;

namespace ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheRemove;

public class MemoryCacheRemoveCategoryHandler(IMemoryCacheStorage cache) : MemoryCacheModuleRequestHandler<MemoryCacheModuleRemoveCategoryCommand, Result<bool>>
{
  private readonly IMemoryCacheStorage _cache = cache ?? throw new ArgumentException($"{nameof(cache)} is null.");

  public override Task<Result<bool>> Handle(MemoryCacheModuleRemoveCategoryCommand request, CancellationToken cancellationToken)
  {
    if (request.MainCategory != null)
      _cache.RemoveCategory(request.MainCategory, request.SubCategory);

    return Task.FromResult(Result.Success(true));
  }
}