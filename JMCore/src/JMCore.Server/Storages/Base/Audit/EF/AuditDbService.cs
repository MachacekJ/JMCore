using JMCore.Server.Storages.Base.Audit.Configuration;
using JMCore.Server.Storages.Base.Audit.Models;
using JMCore.Server.Storages.Base.Audit.UserProvider;
using JMCore.Server.Storages.Modules.AuditModule;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace JMCore.Server.Storages.Base.Audit.EF;

public class AuditDbService(IAuditStorageModule auditModuleEfContext, IAuditConfiguration auditConfiguration, IAuditUserProvider? auditUserProvider, ILogger<AuditDbService> logger)
    : IAuditDbService
{
    private readonly IAuditUserProvider _auditUserProvider = auditUserProvider ?? new AuditEmptyUserProvider();

    public async Task<IEnumerable<AuditEntryItem>> OnBeforeSaveChangesAsync(ChangeTracker changeTracker)
    {
        changeTracker.DetectChanges();
        var auditEntries = (from entry in changeTracker.Entries()
            where entry.ShouldBeAudited(auditConfiguration.AuditEntities)
            select new AuditEntryItem(entry, _auditUserProvider, auditConfiguration, logger)).ToList();

        await BeginTrackingAuditEntriesAsync(auditEntries.Where(e => !e.HasTemporaryProperties));

        // keep a list of entries where the value of some properties are unknown at this step
        return auditEntries.Where(e => e.HasTemporaryProperties);
    }

    public async Task OnAfterSaveChangesAsync(IEnumerable<AuditEntryItem> entityAudits)
    {
        var auditEntries = entityAudits as AuditEntryItem[] ?? entityAudits.ToArray();

        if (!auditEntries.Any())
            return;

        await BeginTrackingAuditEntriesAsync(auditEntries);
    }

    private async Task BeginTrackingAuditEntriesAsync(IEnumerable<AuditEntryItem> auditEntries)
    {
        foreach (var auditEntry in auditEntries)
        {
            auditEntry.Update();
            await SaveAuditAsync(auditEntry);
        }
    }

    private async Task SaveAuditAsync(AuditEntryItem auditEntryItem) => await auditModuleEfContext.SaveAuditAsync(auditEntryItem);
}