using ACore.Models;
using ACore.Server.Modules.AuditModule.Models;

namespace ACore.Server.Modules.AuditModule.CQRS.Audit.AuditSave;

internal class AuditSaveCommand(AuditEntryItem auditEntryItem) : AuditModuleRequest<Result>
{
  public AuditEntryItem AuditEntryItem => auditEntryItem;
}