using ACore.Server.Configuration;
using ACore.Server.Modules.SettingsDbModule.Storage;
using ACore.Server.Storages.Definitions.Models;
using ACore.Tests.Server.Storages;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.Modules.SettingsDbModule;

public class SettingsDbModuleTestsBase : StorageTestsBase
{
  protected ISettingsDbModuleStorage? MemorySettingStorageModule;

  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    sc.AddACoreServer(o =>
    {
      MemoryStorageConfiguration.Invoke(o);
      o.AddSettingModule();
    });
  }

  protected override async Task GetServices(IServiceProvider sp)
  {
    await base.GetServices(sp);
    await sp.UseACoreServer();

    MemorySettingStorageModule = StorageResolver?.FirstReadOnlyStorage<ISettingsDbModuleStorage>(StorageTypeEnum.Memory) ?? throw new ArgumentNullException($"{nameof(ISettingsDbModuleStorage)} is not implemented.");
  }
}