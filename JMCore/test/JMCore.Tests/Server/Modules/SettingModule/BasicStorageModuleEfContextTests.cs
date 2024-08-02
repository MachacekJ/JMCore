using JMCore.Server.MemoryStorage;
using JMCore.Server.Modules.SettingModule.Storage;
using JMCore.Server.Storages.Models;
using JMCore.Tests.Server.Storages;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Tests.Server.Modules.SettingModule;

public class BasicStorageModuleEfContextTests : StorageBaseTests
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
    Db = StorageResolver.FirstReadWriteStorage<IBasicStorageModule>(StorageTypeEnum.Memory);
  }
}