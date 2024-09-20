using Microsoft.EntityFrameworkCore;

namespace ACore.Server.Modules.AuditModule.Models;

public class AuditEntryItem
{
  public string TableName { get; }
  public string? SchemaName { get; }
  public int Version { get; }
  public List<AuditEntryColumnItem> ChangedColumns { get; } = [];
  public EntityState EntityState { get; private set; }
  public string? UserId { get; private set; }
  public long? PkValue { get; private set; }
  public string? PkValueString { get; set; }
  public DateTime Created { get; set; }

  public AuditEntryItem(string tableName, string? schemaName, int version, object pkValue, EntityState entityState, string? userId)
  {
    TableName = tableName;
    SchemaName = schemaName;
    EntityState = entityState;
    Version = version;
    UserId = userId;
    SetPK(pkValue);
  }

  public void AddEntry(string columnName, bool isChanged, object? oldValue, object? newValue)
  {
    ChangedColumns.Add(new AuditEntryColumnItem(columnName, isChanged, oldValue, newValue));
  }

  public void SetEntityState(EntityState entityState)
  {
    EntityState = entityState;
  }

  public void SetPK<TPK>(TPK pkValue)
  {
    ArgumentNullException.ThrowIfNull(pkValue);

    if (long.TryParse(pkValue.ToString(), out var pkv))
      PkValue = pkv;
    else
      PkValueString = pkValue.ToString();
  }

  public TPK? GetPK<TPK>()
  {
    if (PkValue != null)
      return (TPK)Convert.ChangeType(PkValue.Value, typeof(TPK));

    if (PkValueString != null)
      return (TPK)Convert.ChangeType(PkValueString, typeof(TPK));

    return default;
  }
}