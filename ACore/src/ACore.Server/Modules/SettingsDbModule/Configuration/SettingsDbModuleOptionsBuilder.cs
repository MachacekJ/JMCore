using ACore.Base.Modules;
using ACore.Server.Configuration.Modules;
using ACore.Server.Modules.SettingsDbModule.Storage;
using ACore.Server.Storages.Configuration;

namespace ACore.Server.Modules.SettingsDbModule.Configuration;

public class SettingsDbModuleOptionsBuilder : ServerModuleOptionBuilder, IModuleOptionsBuilder
{
  private bool _isActive;
  public static SettingsDbModuleOptionsBuilder Empty() => new();


  public SettingsDbModuleOptions Build(StorageOptionBuilder? defaultStorages)
  {
    return new SettingsDbModuleOptions
    {
      Storages = BuildStorage(defaultStorages, nameof(ISettingsDbStorageModule)),
      IsActive = _isActive
    };
  }

  public void Activate()
  {
    _isActive = true;
  }
}