using JMCore.Server.DB.DbContexts.AuditStructure;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JMCore.Server.DB.Audit;

public class AuditDbService : IAuditDbService
{
  private readonly IAuditDbContext _auditDbContext;
  private readonly IAuditUserProvider _auditUserProvider;
  private readonly IAuditEntitiesConfiguration _auditConfiguration;

  public AuditDbService(IAuditDbContext auditDbContext, IAuditEntitiesConfiguration auditConfiguration, IAuditUserProvider? auditUserProvider)
  {
    _auditDbContext = auditDbContext;
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

    await _auditDbContext.SaveChangesAsync(CancellationToken.None);
  }

  private async Task BeginTrackingAuditEntriesAsync(IEnumerable<AuditEntry> auditEntries)
  {
    foreach (var auditEntry in auditEntries)
    {
      auditEntry.Update();
      await SaveAuditAsync(auditEntry);
    }
  }

  private async Task SaveAuditAsync(AuditEntry auditEntry) => await _auditDbContext.SaveAuditAsync(auditEntry);
}