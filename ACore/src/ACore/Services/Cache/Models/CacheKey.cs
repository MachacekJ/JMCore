namespace ACore.Services.Cache.Models;

public class CacheKey
{
  public TimeSpan? Duration { get; }
  public CacheCategory MainCategory { get; }

  public CacheCategory? SubCategory { get; }
  public string Key { get; }

  private CacheKey(CacheCategory mainCategory, string key, TimeSpan? duration)
  {
    MainCategory = mainCategory;
    Key = key;
    Duration = duration;
  }

  private CacheKey(CacheCategory mainCategory, CacheCategory? subCategory, string key, TimeSpan? duration)
  {
    MainCategory = mainCategory;
    SubCategory = subCategory;
    Key = key;
    Duration = duration;
  }

  public static CacheKey Create(CacheCategory category, string key, TimeSpan? duration = null)
  {
    return new CacheKey(category, key, duration);
  }

  public static CacheKey Create(CacheCategory category, CacheCategory subCategory, string key, TimeSpan? duration = null)
  {
    return new CacheKey(category, subCategory, key, duration);
  }

  public override string ToString()
  {
    return $"C:{MainCategory.CategoryNameKey}^S:{SubCategory?.CategoryNameKey ?? string.Empty}^K:{Key}";
  }
}