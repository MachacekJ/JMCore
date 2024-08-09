using ACore.Server.Configuration.Modules;
using ACore.Server.Modules.SettingsDbModule.Storage;
using ACore.Server.Storages.Configuration;

namespace ACore.Server.Modules.SettingsDbModule.Configuration;

public class SettingsDbModuleOptionsBuilder : StorageModuleOptionBuilder
{

  public static SettingsDbModuleOptionsBuilder Empty() => new();


  public SettingsDbModuleOptions Build(StorageOptionBuilder? defaultStorages)
  {
    return new SettingsDbModuleOptions(IsActive)
    {
      Storages = BuildStorage(defaultStorages, nameof(ISettingsDbModuleStorage)),
    };
  }
}