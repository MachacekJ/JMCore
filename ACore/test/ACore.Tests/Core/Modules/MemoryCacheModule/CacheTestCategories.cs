using ACore.Base.Cache;

namespace ACore.Tests.Core.Modules.MemoryCacheModule;

public static class CacheTestCategories
{
  public static CacheCategory CacheTest => new(nameof(CacheTest));
  public static CacheCategory CacheTest2 => new(nameof(CacheTest2));
}