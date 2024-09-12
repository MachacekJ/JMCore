using ACore.Configuration;
using ACore.Modules.MemoryCacheModule.Storages;
using ACore.Tests.BaseInfrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Core.ServicesT.JMCacheT.JMMemoryCacheT
{
  public class MemoryBaseTests : BaseTests
  {
    protected IMemoryCacheStorage? MemoryCacheStorage { get; private set; }

    protected override void RegisterServices(ServiceCollection sc)
    {
      base.RegisterServices(sc);
      sc.AddACore(o =>
        o.AddMemoryCacheModule(op =>
          op.AddCacheCategories(CacheTestCategoryT.CacheTest)));
    }

    protected override async Task GetServicesAsync(IServiceProvider sp)
    {
      await base.GetServicesAsync(sp);
      MemoryCacheStorage = sp.GetService<IMemoryCacheStorage>() ?? throw new ArgumentException($"{nameof(IMemoryCacheStorage)} is null.");
    }
  }
}