using ACore.Base.CQRS.Results;
using ACore.Server.Modules.AuditModule.CQRS.AuditGet.Models;

namespace ACore.Server.Modules.AuditModule.CQRS.AuditGet;

public class AuditGetQuery<TPK>(string tableName, TPK pkValue, bool restoreEntity = false , string? schemaName = null) : AuditModuleRequest<Result<AuditGetQueryDataOut<TPK>[]>>
{
  public string TableName => tableName;
  public string? SchemaName => schemaName;
  public TPK PKValue => pkValue;
  public bool RestoreEntity => restoreEntity;
}