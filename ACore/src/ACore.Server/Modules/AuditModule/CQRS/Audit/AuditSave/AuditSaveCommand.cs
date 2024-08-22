using ACore.Server.Modules.AuditModule.Models;
using MediatR;

namespace ACore.Server.Modules.AuditModule.CQRS.Audit.AuditSave;

internal class AuditSaveCommand(AuditEntryItem auditEntryItem) : IRequest
{
  public AuditEntryItem AuditEntryItem => auditEntryItem;
}