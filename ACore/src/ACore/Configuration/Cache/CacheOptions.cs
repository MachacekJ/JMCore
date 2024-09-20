using ACore.Base.Cache;

namespace ACore.Configuration.Cache;

public class CacheOptions
{
  public List<CacheCategory> Categories { get; init; } = [];
}