using JMCore.Server.Storages.Base.Audit.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JMCore.Server.Storages.Base.Audit.EF;

public interface IAuditDbService
{
    public Task<IEnumerable<AuditEntry>> OnBeforeSaveChangesAsync(ChangeTracker changeTracker);
    Task OnAfterSaveChangesAsync(IEnumerable<AuditEntry> entityAudits);
}

