using ACore.Server.Configuration.Modules;
using ACore.Server.Modules.AuditModule.UserProvider;

namespace ACore.Server.Modules.AuditModule.Configuration;

public class AuditModuleOptions(bool isActive = false) : StorageModuleOptions(nameof(AuditModule), isActive)
{
  public IAuditUserProvider? AuditUserProvider { get; init; }
}