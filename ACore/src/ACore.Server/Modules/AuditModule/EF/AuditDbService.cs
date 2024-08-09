using ACore.Server.Modules.AuditModule.Models;
using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Modules.AuditModule.UserProvider;

namespace ACore.Server.Modules.AuditModule.EF;

public class AuditDbService(IAuditStorageModule auditModuleEfContext, IAuditUserProvider? auditUserProvider)
  : IAuditDbService
{
  public IAuditUserProvider AuditUserProvider => auditUserProvider ?? new AuditEmptyUserProvider();

  public async Task SaveAuditAsync(AuditEntryItem auditEntryItem) => await auditModuleEfContext.SaveAuditAsync(auditEntryItem);
}