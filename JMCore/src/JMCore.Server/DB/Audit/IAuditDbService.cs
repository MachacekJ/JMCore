using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JMCore.Server.DB.Audit;

public interface IAuditDbService
{
    public Task<IEnumerable<AuditEntry>> OnBeforeSaveChangesAsync(ChangeTracker changeTracker);
    Task OnAfterSaveChangesAsync(IEnumerable<AuditEntry> entityAudits);
}

