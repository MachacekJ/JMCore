using ACore.Server.Configuration.Modules;

namespace ACore.Server.Modules.AuditModule.Configuration;

public class AuditModuleOptions(bool isActive) : ModuleOptions(isActive)
{
  public IAuditConfiguration? ManualConfiguration { get; init; } = null;
}