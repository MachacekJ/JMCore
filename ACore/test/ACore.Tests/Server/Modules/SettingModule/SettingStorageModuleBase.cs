using ACore.Server.Configuration;
using ACore.Server.Modules.SettingModule.Storage;
using ACore.Server.Storages.Models;
using ACore.Tests.Server.Storages;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.Modules.SettingModule;

public class SettingStorageModule : StorageBase
{
  protected ISettingStorageModule? MemorySettingStorageModule;

  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    sc.AddACoreServer(o =>
    {
      StorageConfiguration.Invoke(o);
      o.AddSettingModule();
    });
  }

  protected override async Task GetServicesAsync(IServiceProvider sp)
  {
    await base.GetServicesAsync(sp);
    await sp.UseACoreServer();

    MemorySettingStorageModule = StorageResolver?.FirstReadOnlyStorage<ISettingStorageModule>(StorageTypeEnum.Memory) ?? throw new ArgumentNullException($"{nameof(ISettingStorageModule)} is not implemented.");
  }
}