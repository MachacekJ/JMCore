namespace ACore.Services.Cache;

/// <summary>
/// Cache category. 
/// </summary>
/// <param name="categoryNameKey">It must be unique in all applications.</param>
/// <param name="duration">How long the value will be cached in seconds.</param>
public class CacheCategory(string categoryNameKey, TimeSpan? duration = null)
{
    private readonly TimeSpan _defaultDuration = TimeSpan.FromMinutes(60);
    public string CategoryNameKey => categoryNameKey;
    public TimeSpan Duration => duration ?? _defaultDuration;
}