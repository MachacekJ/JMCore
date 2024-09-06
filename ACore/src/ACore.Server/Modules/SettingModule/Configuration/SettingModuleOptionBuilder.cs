using ACore.Server.Storages.Configuration;

namespace ACore.Server.Modules.SettingModule.Configuration;

public class SettingModuleOptionBuilder
{
  private StorageOptionBuilder _storageOptionBuilder = StorageOptionBuilder.Empty();

  public static SettingModuleOptionBuilder Empty() => new();


  public SettingModuleOptions Build()
  {
    return new SettingModuleOptions();
  }
}