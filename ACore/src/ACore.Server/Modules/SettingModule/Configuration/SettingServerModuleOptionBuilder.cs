using ACore.Base;
using ACore.Base.Modules;
using ACore.Server.Configuration.Modules;
using ACore.Server.Modules.SettingModule.Storage;
using ACore.Server.Storages.Configuration;

namespace ACore.Server.Modules.SettingModule.Configuration;

public class SettingServerModuleOptionBuilder : ServerModuleOptionBuilder, IModuleOptionsBuilder
{
  private bool _isActive;
  public static SettingServerModuleOptionBuilder Empty() => new();


  public SettingServerModuleOptions Build(StorageOptionBuilder? defaultStorages)
  {
    return new SettingServerModuleOptions
    {
      Storages = BuildStorage(defaultStorages, nameof(ISettingStorageModule)),
      IsActive = _isActive
    };
  }

  public void Activate()
  {
    _isActive = true;
  }
}