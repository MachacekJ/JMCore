using JMCore.Server.MemoryStorage;
using JMCore.Server.Modules.SettingModule.Storage;
using JMCore.Server.Storages.Models;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Tests.ServerT.StoragesT.ModulesT.BasicStorageT;

public class BasicStorageModuleEfContextT : DbBaseT
{
  protected IBasicStorageModule Db = null!;

  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    StorageResolver.RegisterStorage(sc, new MemoryStorageConfiguration(new[] { nameof(IBasicStorageModule) }));
  }

  protected override async Task GetServicesAsync(IServiceProvider sp)
  {
    await base.GetServicesAsync(sp);
    Db = StorageResolver.FirstStorageModuleImplementation<IBasicStorageModule>(StorageTypeEnum.Memory);
  }
}