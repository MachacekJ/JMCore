using ACore.Server.MemoryStorage;
using ACore.Server.Modules.SettingModule.Storage;
using ACore.Server.Storages.Models;
using ACore.Tests.Server.Storages;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.Modules.SettingModule;

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