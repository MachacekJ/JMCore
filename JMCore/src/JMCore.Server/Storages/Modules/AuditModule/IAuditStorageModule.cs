using JMCore.Server.Storages.Base.Audit.Models;
using JMCore.Server.Storages.Base.EF;

namespace JMCore.Server.Storages.Modules.AuditModule;

public interface IAuditStorageModule : IDbContextBase, IStorage
{
  Task SaveAuditAsync(AuditEntry auditEntry);
}