using ACore.Configuration;
using ACore.Server.Configuration.Modules;


namespace ACore.Server.Modules.SettingModule.Configuration;

public class SettingServerModuleOptions : ServerModuleOptions, IModuleOptions
{
  public bool IsActive { get; init; }
  public string ModuleName => "SettingServerModule";
}