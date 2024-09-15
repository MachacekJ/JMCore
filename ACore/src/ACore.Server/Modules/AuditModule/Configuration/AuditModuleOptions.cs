using ACore.Base.Modules;
using ACore.Server.Configuration.Modules;
using ACore.Server.Modules.AuditModule.UserProvider;

namespace ACore.Server.Modules.AuditModule.Configuration;

public class AuditModuleOptions: StorageModuleOptions, IModuleOptions
{
  public bool IsActive { get; init; }
  public string ModuleName => nameof(AuditModule);
  public IAuditUserProvider? AuditUserProvider { get; init; }
}