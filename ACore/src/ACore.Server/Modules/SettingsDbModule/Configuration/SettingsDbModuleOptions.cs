using ACore.Base.Modules;
using ACore.Server.Configuration.Modules;

namespace ACore.Server.Modules.SettingsDbModule.Configuration;

public class SettingsDbModuleOptions : StorageModuleOptions, IModuleOptions
{
  public bool IsActive { get; init; }
  public string ModuleName => nameof(SettingsDbModule);
}