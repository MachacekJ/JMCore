using ACore.Server.Modules.AuditModule.CQRS.Models;

namespace ACore.Server.Modules.AuditModule.CQRS.Audit;

public class AuditGetQuery<T>(string tableName, T pkValue, string? schemaName = null) : AuditModuleRequest<AuditValueData[]>
{
  public string TableName => tableName;
  public string? SchemaName => schemaName;
  public T PKValue => pkValue;
}