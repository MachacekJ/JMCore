using JMCore.Server.Configuration.Storage;
using JMCore.Server.Services.JMCache;
using JMCore.Server.Storages.Modules.BasicModule;
using JMCore.Services.JMCache;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Tests.ServerT.StoragesT;

/// <summary>
/// Working with memory EF.
/// </summary>
public class DbBaseT : ServerTestBaseT
{
  protected readonly IStorageResolver StorResolver = new StorageResolver();

  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    sc.AddSingleton(StorResolver);

    sc.AddJMMemoryCache<JMCacheServerCategory>();
    sc.AddMediatR((c) => { c.RegisterServicesFromAssemblyContaining(typeof(IBasicStorageModule)); });
  }

  protected override async Task GetServicesAsync(IServiceProvider sp)
  {
    await StorResolver.ConfigureStorages(sp);
    await base.GetServicesAsync(sp);
  }
}