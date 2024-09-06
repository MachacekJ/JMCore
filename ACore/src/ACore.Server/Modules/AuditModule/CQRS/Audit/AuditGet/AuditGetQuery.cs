using ACore.Models;
using ACore.Server.Modules.AuditModule.CQRS.Audit.AuditGet.Models;

namespace ACore.Server.Modules.AuditModule.CQRS.Audit.AuditGet;

public class AuditGetQuery<T>(string tableName, T pkValue, string? schemaName = null) : AuditModuleRequest<Result<AuditGetQueryDataOut[]>>
{
  public string TableName => tableName;
  public string? SchemaName => schemaName;
  public T PKValue => pkValue;
}