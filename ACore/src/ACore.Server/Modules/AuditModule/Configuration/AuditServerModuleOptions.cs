using ACore.Base;
using ACore.Base.Modules;
using ACore.Configuration;
using ACore.Server.Configuration.Modules;

namespace ACore.Server.Modules.AuditModule.Configuration;

public class AuditServerModuleOptions: ServerModuleOptions, IModuleOptions
{
  public bool IsActive { get; init; }
  public string ModuleName => GlobalNames.AuditModule;
  public IAuditConfiguration? ManualConfiguration { get; init; }
}