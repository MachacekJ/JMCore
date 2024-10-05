using ACore.Server.Modules.AuditModule.Models;
using ACore.Server.Storages;
using ACore.Server.Storages.Models.SaveInfo;

namespace ACore.Server.Modules.AuditModule.Storage;

public interface IAuditStorageModule : IStorage
{
  Task SaveAuditAsync(SaveInfoItem saveInfoItem);

  Task<AuditInfoItem[]> AuditItemsAsync<T>(string tableName, T pkValue, string? schemaName = null);
}