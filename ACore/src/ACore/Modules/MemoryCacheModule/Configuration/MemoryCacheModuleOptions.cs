using ACore.Base;
using ACore.Base.Modules;
using ACore.Configuration.Cache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace ACore.Modules.MemoryCacheModule.Configuration;

public class MemoryCacheModuleOptions : CacheOptions, IOptions<MemoryCacheModuleOptions>, IModuleOptions
{
  public Action<MemoryCacheOptions>? MemoryCacheOptionAction { get; init; }

  public MemoryCacheModuleOptions Value => this;
  public bool IsActive { get; init; }
  public string ModuleName => "MemoryCacheModule";
}