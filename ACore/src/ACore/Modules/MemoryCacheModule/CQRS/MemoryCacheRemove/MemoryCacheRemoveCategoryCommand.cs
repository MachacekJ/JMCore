using ACore.Models;
using ACore.Services.Cache;

namespace ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheRemove;

public class MemoryCacheModuleRemoveCategoryCommand(CacheCategory mainCategory, CacheCategory? subCategory = null) : MemoryCacheModuleRequest<Result<bool>>
{
  public CacheCategory? MainCategory { get; } = mainCategory;
  public CacheCategory? SubCategory { get; } = subCategory;
}