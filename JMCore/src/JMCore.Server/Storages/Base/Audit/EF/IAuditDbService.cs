using JMCore.Server.Storages.Base.Audit.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JMCore.Server.Storages.Base.Audit.EF;

public interface IAuditDbService
{
    public Task<IEnumerable<AuditEntryItem>> OnBeforeSaveChangesAsync(ChangeTracker changeTracker);
    Task OnAfterSaveChangesAsync(IEnumerable<AuditEntryItem> entityAudits);
}

