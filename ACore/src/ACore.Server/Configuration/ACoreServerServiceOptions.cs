using ACore.Configuration;
using ACore.Server.Modules.SettingModule.Configuration;
using ACore.Server.Storages.Configuration.Options;

namespace ACore.Server.Configuration;

public class ACoreServerServiceOptions
{
  public ACoreServiceOptions ACoreServiceOptions { get; init; } = new ();

  /// <summary>
  /// All modules using the storage take this default setting first.
  /// </summary>
  public ACoreStorageOptions? DefaultStorages { get; init; } = null;
  
  public SettingModuleOptions SettingModuleOptions { get; init; } = new();
}