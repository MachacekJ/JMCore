using ACore.Configuration;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.SettingModule.Configuration;
using ACore.Server.Storages.Configuration;

namespace ACore.Server.Configuration;

public class ACoreServerServiceOptions
{
  public ACoreServiceOptions ACoreServiceOptions { get; init; } = new ();

  /// <summary>
  /// All modules using the storage take this default setting first.
  /// </summary>
  public StorageOptions? DefaultStorages { get; init; } = null;
  
  public SettingModuleOptions SettingModuleOptions { get; init; } = new(false);

  public AuditModuleOptions AuditModuleOptions { get; init; } = new(false);
}