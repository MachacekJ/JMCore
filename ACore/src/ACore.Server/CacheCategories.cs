using ACore.Base.Cache;

namespace ACore.Server;

public static class CacheCategories
{
  public static CacheCategory Entity => new(nameof(Entity));
}