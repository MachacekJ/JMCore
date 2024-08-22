using ACore.Server.Modules.AuditModule.Models;
using ACore.Server.Storages;

namespace ACore.Server.Modules.AuditModule.Storage;

public interface IAuditStorageModule : IStorage
{
  Task SaveAuditAsync(AuditEntryItem auditEntryItem);

  Task<AuditEntryItem[]> AuditItemsAsync<T>(string tableName, T pkValue, string? schemaName = null);
}