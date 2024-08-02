using ACore.Server.Modules.AuditModule.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ACore.Server.Modules.AuditModule.EF;

public interface IAuditDbService
{
    public Task<IEnumerable<AuditEntryItem>> OnBeforeSaveChangesAsync(ChangeTracker changeTracker);
    Task OnAfterSaveChangesAsync(IEnumerable<AuditEntryItem> entityAudits);
}

