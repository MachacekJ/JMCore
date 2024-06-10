using JMCore.Server.Storages.Base.Audit.Configuration;
using JMCore.Server.Storages.Base.Audit.Models;
using JMCore.Server.Storages.Base.Audit.UserProvider;
using JMCore.Server.Storages.Modules.AuditModule;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JMCore.Server.Storages.Base.Audit.EF;

public class AuditDbService : IAuditDbService
{
    private readonly IAuditStorageModule _auditModuleEfContext;
    private readonly IAuditUserProvider _auditUserProvider;
    private readonly IAuditConfiguration _auditConfiguration;

    public AuditDbService(IAuditStorageModule auditModuleEfContext, IAuditConfiguration auditConfiguration, IAuditUserProvider? auditUserProvider)
    {
        _auditModuleEfContext = auditModuleEfContext;
        _auditUserProvider = auditUserProvider ?? new AuditEmptyUserProvider();
        _auditConfiguration = auditConfiguration;
    }

    public async Task<IEnumerable<AuditEntry>> OnBeforeSaveChangesAsync(ChangeTracker changeTracker)
    {
        changeTracker.DetectChanges();
        var auditEntries = (from entry in changeTracker.Entries()
            where entry.ShouldBeAudited(_auditConfiguration.AuditEntities)
            select new AuditEntry(entry, _auditUserProvider, _auditConfiguration)).ToList();

        await BeginTrackingAuditEntriesAsync(auditEntries.Where(e => !e.HasTemporaryProperties));

        // keep a list of entries where the value of some properties are unknown at this step
        return auditEntries.Where(e => e.HasTemporaryProperties);
    }

    public async Task OnAfterSaveChangesAsync(IEnumerable<AuditEntry> entityAudits)
    {
        var auditEntries = entityAudits as AuditEntry[] ?? entityAudits.ToArray();

        if (!auditEntries.Any())
            return;

        await BeginTrackingAuditEntriesAsync(auditEntries);
        // foreach (var auditEntry in auditEntries)
        // {
        //     await _auditModuleEfContext.SaveAuditAsync(auditEntry);
        // }

    }

    private async Task BeginTrackingAuditEntriesAsync(IEnumerable<AuditEntry> auditEntries)
    {
        foreach (var auditEntry in auditEntries)
        {
            auditEntry.Update();
            await SaveAuditAsync(auditEntry);
        }
    }

    private async Task SaveAuditAsync(AuditEntry auditEntry) => await _auditModuleEfContext.SaveAuditAsync(auditEntry);
}