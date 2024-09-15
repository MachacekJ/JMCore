using ACore.Configuration;
using ACore.Modules.MemoryCacheModule.Storages;
using ACore.Server.Configuration;
using ACore.Tests.BaseInfrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Core.Modules.MemoryCacheModule
{
  public class MemoryCacheModuleBaseTests(bool onlyCacheTest = false) : BaseTests
  {
    protected IMemoryCacheModuleStorage? MemoryCacheStorage { get; private set; }

    protected override void RegisterServices(ServiceCollection sc)
    {
      base.RegisterServices(sc);
      sc.AddACore(o =>
        o.AddMemoryCacheModule(op =>
          op.AddCacheCategories(
            CacheTestCategories.CacheTest,
            CacheTestCategories.CacheTest2
          )
        ));
    }

    protected override async Task GetServices(IServiceProvider sp)
    {
      await base.GetServices(sp);
      await sp.UseACoreServer();
      MemoryCacheStorage = sp.GetService<IMemoryCacheModuleStorage>() ?? throw new ArgumentException($"{nameof(IMemoryCacheModuleStorage)} is null.");
    }
  }
}