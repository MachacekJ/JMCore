using JMCore.Server.Storages.Abstract;
using JMCore.Server.Storages.Audit;

namespace JMCore.Server.Storages.DbContexts.AuditStructure;

public interface IAuditDbContext : IDbContextBase
{
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task SaveAuditAsync(AuditEntry auditEntry);
}