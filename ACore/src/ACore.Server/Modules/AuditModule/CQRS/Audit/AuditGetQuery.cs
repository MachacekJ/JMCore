using ACore.Server.Modules.AuditModule.CQRS.Models;

namespace ACore.Server.Modules.AuditModule.CQRS.Audit;

public class AuditGetQuery<T> : AuditModuleRequest<AuditValueData[]>
{
  private readonly string _tableName;
  private readonly T _pkValue;
  private readonly string? _schemaName;
  
  public string TableName => _tableName;
  public string? SchemaName => _schemaName;
  public T PKValue => _pkValue;
  

  public AuditGetQuery(string tableName, T pkValue, string? schemaName = null)
  {
    _tableName = tableName;
    _pkValue = pkValue;
    _schemaName = schemaName;
  }
}