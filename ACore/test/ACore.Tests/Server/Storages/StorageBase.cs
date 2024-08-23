using ACore.Server.Services.JMCache;
using ACore.Server.Storages;
using ACore.Modules.CacheModule;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.Storages;

/// <summary>
/// Working with memory EF.
/// </summary>
public class StorageBase : ServerBase
{
  protected IStorageResolver? StorageResolver;

  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);

    sc.AddJMMemoryCache<JMCacheServerCategory>();
  }

  protected override async Task GetServicesAsync(IServiceProvider sp)
  {
    await base.GetServicesAsync(sp);
    StorageResolver = sp.GetService<IStorageResolver>() ?? throw new ArgumentNullException($"{nameof(IStorageResolver)} not found.");
  }
}