using ACore.Server.Configuration;
using ACore.Server.Modules.SettingsDbModule.Storage;
using ACore.Server.Storages.Models;
using ACore.Tests.Server.Storages;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.Modules.SettingModule;

public class SettingStorageModule : StorageBase
{
  protected ISettingsDbStorageModule? MemorySettingStorageModule;

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

    MemorySettingStorageModule = StorageResolver?.FirstReadOnlyStorage<ISettingsDbStorageModule>(StorageTypeEnum.Memory) ?? throw new ArgumentNullException($"{nameof(ISettingsDbStorageModule)} is not implemented.");
  }
}