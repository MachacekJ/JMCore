using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Storages.Modules;
using JMCore.Server.Storages.Modules.BasicModule;
using JMCore.Tests.ServerT.StoragesT.Impl.MemoryStorage;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Tests.ServerT.StoragesT.BasicStorageT;

public class BasicStorageModuleEfContextT : DbBaseT
{
  protected IBasicStorageModule Db = null!;

  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    StorResolver.RegisterStorage(sc, new MemoryStorageConfiguration(new[] { nameof(IBasicStorageModule) }));
  }

  protected override async Task GetServicesAsync(IServiceProvider sp)
  {
    await base.GetServicesAsync(sp);
    Db = StorResolver.FirstStorageModuleImplementation<IBasicStorageModule>(StorageTypeEnum.Memory);
  }
}