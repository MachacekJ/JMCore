using ACore.Server.Configuration.Modules;
using ACore.Server.Modules.SettingModule.Storage;
using ACore.Server.Storages.Configuration;

namespace ACore.Server.Modules.SettingModule.Configuration;

public class SettingModuleOptionBuilder : ModuleOptionBuilder
{
  public static SettingModuleOptionBuilder Empty() => new();


  public SettingModuleOptions Build(StorageOptionBuilder? defaultStorages)
  {
    return new SettingModuleOptions(IsActive)
    {
      Storages = BuildStorage(defaultStorages, nameof(ISettingStorageModule)),
    };
  }
}