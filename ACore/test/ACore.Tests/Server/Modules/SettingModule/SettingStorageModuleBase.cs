using ACore.Configuration;
using ACore.Server.Configuration;
using ACore.Server.Modules.SettingModule;
using ACore.Server.Modules.SettingModule.Storage;
using ACore.Server.Storages.Configuration.Options;
using ACore.Server.Storages.Models;
using ACore.Tests.Server.Storages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ACore.Tests.Server.Modules.SettingModule;

public class SettingStorageModule : StorageBase
{
  protected ISettingStorageModule? MemorySettingStorageModule;

  private readonly ACoreStorageOptions _aCoreStorageOptions = new()
  {
    UseMemoryStorage = true
  };

  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    sc.AddACoreServer(o =>
    {
      o.ACoreServiceOptions.Name = "yyyy";
      o.ServerName = "xxxx";
    });

    sc.AddSettingServiceModule(_aCoreStorageOptions);
  }

  protected override async Task GetServicesAsync(IServiceProvider sp)
  {
    await base.GetServicesAsync(sp);
    await sp.UseSettingServiceModule(_aCoreStorageOptions);

    var aa = sp.GetService<IOptions<ACoreServiceOptions>>();

    MemorySettingStorageModule = StorageResolver?.FirstReadOnlyStorage<ISettingStorageModule>(StorageTypeEnum.Memory) ?? throw new ArgumentNullException($"{nameof(ISettingStorageModule)} is not implemented.");
  }
}