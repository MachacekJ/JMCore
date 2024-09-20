namespace ACore.Base.Cache;

/// <summary>
/// Key has 3 levels
/// 1 - Main category (required)
/// 2 - Sub category (optional)
/// 3 - Key
/// </summary>
public class CacheKey
{
  public TimeSpan? Duration { get; }
  public CacheCategory MainCategory { get; }

  private CacheCategory? SubCategory { get; }
  private string Key { get; }

  private CacheKey(CacheCategory mainCategory, string key, TimeSpan? duration) : this(mainCategory, null, key, duration)
  {
  }

  private CacheKey(CacheCategory mainCategory, CacheCategory? subCategory, string key, TimeSpan? duration)
  {
    CheckForbiddenLettres(mainCategory.CategoryNameKey);
    if (subCategory != null)
      CheckForbiddenLettres(subCategory.CategoryNameKey);
    
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

  private void CheckForbiddenLettres(string key)
  {
    if (key.Contains('^'))
      throw new Exception($"Cache key '{key}' contains forbidden letter '^'.");
  }
}