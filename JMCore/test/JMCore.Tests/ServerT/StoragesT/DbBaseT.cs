using JMCore.Server.Configuration.Storage;
using JMCore.Server.Services.JMCache;
using JMCore.Services.JMCache;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Tests.ServerT.StoragesT;

/// <summary>
/// Working with memory EF.
/// </summary>
public class DbBaseT : ServerTestBaseT
{
  protected readonly IStorageResolver StorageResolver = new StorageResolver();

  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    StorageResolver.RegisterServices(sc);
    sc.AddSingleton(StorageResolver);
    sc.AddJMMemoryCache<JMCacheServerCategory>();
  }

  protected override async Task GetServicesAsync(IServiceProvider sp)
  {
    await StorageResolver.ConfigureStorages(sp);
    await base.GetServicesAsync(sp);
  }
}