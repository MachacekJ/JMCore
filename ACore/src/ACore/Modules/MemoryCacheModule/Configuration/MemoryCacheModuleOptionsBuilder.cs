using ACore.Base;
using ACore.Services.Cache.Configuration;
using Microsoft.Extensions.Caching.Memory;

namespace ACore.Modules.MemoryCacheModule.Configuration;

public class MemoryCacheModuleOptionsBuilder : CacheOptionsBuilder, IModuleOptionsBuilder
{
  private Action<MemoryCacheOptions>? _memoryCacheOptionsAction;
  private bool _isActive = false;
  
  public static MemoryCacheModuleOptionsBuilder Empty() => new();

  private MemoryCacheModuleOptionsBuilder()
  {
  }
  
  public MemoryCacheModuleOptionsBuilder ConfigureMemoryCache(Action<MemoryCacheOptions>? memoryCacheOptionsAction = null)
  {
    _memoryCacheOptionsAction = memoryCacheOptionsAction;
    return this;
  }

  public MemoryCacheModuleOptions Build()
  {
    var baseBuild = BuildBase();
    return new MemoryCacheModuleOptions
    {
      MemoryCacheOptionAction = _memoryCacheOptionsAction,
      Categories = baseBuild.Categories,
      IsActive = _isActive
    };
  }

  public void Activate()
  {
    _isActive = true;
  }
}