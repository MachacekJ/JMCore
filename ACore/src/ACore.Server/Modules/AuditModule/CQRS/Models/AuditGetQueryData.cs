namespace ACore.Server.Modules.AuditModule.CQRS.Models;

public class AuditGetQueryData
{
  public AuditGetQueryData(string tableName)
  {
    TableName = tableName;
  }

  public string TableName { get; set; }
  public string? SchemaName { get; set; }
  public long? PKValue { get; set; }
  public string? PKValueString { get; set; }
}