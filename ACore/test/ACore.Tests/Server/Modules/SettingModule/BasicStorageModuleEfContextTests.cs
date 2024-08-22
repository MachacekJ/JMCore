using ACore.Server.Configuration;
using ACore.Server.Modules.SettingModule;
using ACore.Server.Modules.SettingModule.Storage;
using ACore.Server.Storages.Models;
using ACore.Tests.Server.Storages;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.Modules.SettingModule;

public class BasicStorageModuleEfContextTests : StorageBaseTests
{
  protected IBasicStorageModule Db = null!;

  private readonly StorageModuleConfiguration _storageModuleConfiguration = new()
  {
    UseMemoryStorage = true
  };

  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    sc.AddSettingServiceModule(_storageModuleConfiguration);
  }

  protected override async Task GetServicesAsync(IServiceProvider sp)
  {
    await base.GetServicesAsync(sp);
    await sp.UseSettingServiceModule(_storageModuleConfiguration);
    Db = StorageResolver?.FirstReadOnlyStorage<IBasicStorageModule>(StorageTypeEnum.Memory) ?? throw new ArgumentNullException($"{nameof(IBasicStorageModule)} is not implemented.");
  }
}