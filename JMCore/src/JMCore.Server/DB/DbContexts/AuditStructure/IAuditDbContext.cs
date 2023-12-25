using JMCore.Server.DB.Abstract;
using JMCore.Server.DB.Audit;

namespace JMCore.Server.DB.DbContexts.AuditStructure;

public interface IAuditDbContext : IDbContextBase
{
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task SaveAuditAsync(AuditEntry auditEntry);
}