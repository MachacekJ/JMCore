using ACore.Server.Modules.AuditModule.Models;
using ACore.Server.Modules.AuditModule.Storage.Models;
using ACore.Server.Storages;

namespace ACore.Server.Modules.AuditModule.Storage;

public interface IAuditStorageModule : IStorage
{
  Task SaveAuditAsync(AuditEntryItem auditEntryItem);

  Task<IEnumerable<AuditValueEntity>> AuditItemsAsync(string tableName, long pkValue, string? schemaName = null);
  Task<IEnumerable<AuditValueEntity>> AuditItemsAsync(string tableName, string pkValue, string? schemaName = null);
}