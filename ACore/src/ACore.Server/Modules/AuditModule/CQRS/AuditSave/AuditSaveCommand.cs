using ACore.Base.CQRS.Results;
using ACore.Server.Modules.AuditModule.Models;

namespace ACore.Server.Modules.AuditModule.CQRS.AuditSave;

internal class AuditSaveCommand(AuditEntryItem auditEntryItem) : AuditModuleRequest<Result>
{
  public AuditEntryItem AuditEntryItem => auditEntryItem;
}