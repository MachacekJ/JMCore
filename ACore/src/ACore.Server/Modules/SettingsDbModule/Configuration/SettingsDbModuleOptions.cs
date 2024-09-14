using ACore.Base.Modules;
using ACore.Server.Configuration.Modules;

namespace ACore.Server.Modules.SettingsDbModule.Configuration;

public class SettingsDbModuleOptions : ServerModuleOptions, IModuleOptions
{
  public bool IsActive { get; init; }
  public string ModuleName => "SettingServerModule";
}