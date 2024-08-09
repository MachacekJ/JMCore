using ACore.Base.Cache;
using ACore.Base.Modules.Configuration;

namespace ACore.Configuration.Cache;

public class CacheOptionsBuilder : ModuleOptionsBuilder
{
  private readonly List<CacheCategory> _categories = [];

  public CacheOptionsBuilder AddCacheCategories(params CacheCategory[] cacheCategories)
  {
    foreach (var cacheCategory in cacheCategories)
    {
      ValidateCacheCategory(cacheCategory);
    }

    _categories.AddRange(cacheCategories);
    return this;
  }

  protected CacheOptions BuildBase()
  {
    return new CacheOptions()
    {
      Categories = _categories
    };
  }

  private void ValidateCacheCategory(CacheCategory cacheCategory)
  {
    // Check forbidden letters.
    if (cacheCategory.CategoryNameKey.Contains("^"))
      throw new Exception($"Cache category key '{cacheCategory.CategoryNameKey}' contains forbidden key '^'.");

    // Check duplicity key.
    if (_categories.Any(e => e.CategoryNameKey == cacheCategory.CategoryNameKey))
      throw new Exception($"Cache category key '{cacheCategory.CategoryNameKey}' is already used. Key must be unique.");
  }
}