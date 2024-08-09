using ACore.Configuration;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.ICAMModule.Configuration;
using ACore.Server.Modules.SettingsDbModule.Configuration;
using ACore.Server.Storages.Configuration;

namespace ACore.Server.Configuration;

public class ACoreServerOptions
{
  public ACoreOptions ACoreOptions { get; init; } = new ();
  
  public StorageOptions? DefaultStorages { get; init; }
  
  public SettingsDbModuleOptions SettingsDbModuleOptions { get; init; } = new();

  public AuditModuleOptions AuditModuleOptions { get; init; } = new();

  public ICAMModuleOptions ICAMModuleOptions { get; init; } = new();
}