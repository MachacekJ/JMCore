using ACore.Server.Modules.AuditModule.CQRS.Models;

namespace ACore.Server.Modules.AuditModule.CQRS.Audit;

public class AuditGetQuery : AuditModuleRequest<AuditValueData[]>
{
  private readonly string _tableName;
  private readonly long? _pkValue;
  private readonly string? _pkStringValue;
  private readonly string? _schemaName;
  
  public string TableName => _tableName;
  public string? SchemaName => _schemaName;
  public long? PKValue => _pkValue;

  public string? PKStringValue => _pkStringValue;

  public AuditGetQuery(string tableName, long pkValue, string? schemaName = null)
  {
    _tableName = tableName;
    _pkValue = pkValue;
    _schemaName = schemaName;
  }

  public AuditGetQuery(string tableName, string pkStringValue, string? schemaName = null)
  {
    _tableName = tableName;
    _pkStringValue = pkStringValue;
    _schemaName = schemaName;
  }
}