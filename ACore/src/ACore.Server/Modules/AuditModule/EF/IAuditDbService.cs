using ACore.Server.Modules.AuditModule.Models;
using ACore.Server.Modules.AuditModule.UserProvider;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ACore.Server.Modules.AuditModule.EF;

public interface IAuditDbService
{
  IAuditUserProvider AuditUserProvider { get; }

  //   public Task<IEnumerable<AuditEntryItem>> OnBeforeSaveChangesAsync(ChangeTracker changeTracker);
  //  Task OnAfterSaveChangesAsync(IEnumerable<AuditEntryItem> entityAudits);
  Task SaveAuditAsync(AuditEntryItem auditEntryItem);
}