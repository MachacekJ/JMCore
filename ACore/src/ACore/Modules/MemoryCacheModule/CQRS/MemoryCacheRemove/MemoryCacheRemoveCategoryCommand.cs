using ACore.Base.Cache;
using ACore.Base.CQRS.Models;
using ACore.Models;

namespace ACore.Modules.MemoryCacheModule.CQRS.MemoryCacheRemove;

public class MemoryCacheModuleRemoveCategoryCommand(CacheCategory mainCategory, CacheCategory? subCategory = null) : MemoryCacheModuleRequest<Result<bool>>
{
  public CacheCategory? MainCategory { get; } = mainCategory;
  public CacheCategory? SubCategory { get; } = subCategory;
}