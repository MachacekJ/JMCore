using ACore.Base.CQRS.Results;
using ACore.Server.Modules.AuditModule.CQRS.AuditGet.Models;
using ACore.Server.Storages.Definitions.Models.PK;

namespace ACore.Server.Modules.AuditModule.CQRS.AuditGet;

public class AuditGetQuery<TEntity, TPK>(string tableName, TPK pkValue, bool restoreEntity = false , string? schemaName = null) : AuditModuleRequest<Result<AuditGetQueryDataOut<TEntity, TPK>[]>>
  where TEntity : PKEntity<TPK>
{
  public string TableName => tableName;
  public string? SchemaName => schemaName;
  public TPK PKValue => pkValue;
  public bool RestoreEntity => restoreEntity;
}