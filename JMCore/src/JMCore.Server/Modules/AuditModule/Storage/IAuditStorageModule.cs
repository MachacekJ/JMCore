using JMCore.Server.Modules.AuditModule.Models;
using JMCore.Server.Modules.AuditModule.Storage.Models;

namespace JMCore.Server.Modules.AuditModule.Storage;

public interface IAuditStorageModule
{
  Task SaveAuditAsync(AuditEntryItem auditEntryItem);
  
  Task<IEnumerable<AuditVwAuditEntity>> AuditItemsAsync(string tableName, int pkValue, string? schemaName = null);
  Task<IEnumerable<AuditVwAuditEntity>> AuditItemsAsync(string tableName, string pkValue, string? schemaName = null);

  /// <summary>
  /// Use this function for testing purposes only.
  /// </summary>
  Task<IEnumerable<AuditVwAuditEntity>> AllAuditItemsAsync(string tableName, string? schemaName = null);
}