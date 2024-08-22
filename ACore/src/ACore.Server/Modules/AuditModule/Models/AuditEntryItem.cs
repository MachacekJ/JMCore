using Microsoft.EntityFrameworkCore;

namespace ACore.Server.Modules.AuditModule.Models;

public class AuditEntryItem
{
  public string TableName { get; }
  public string? SchemaName { get; }
  public List<AuditEntryColumnItem> ChangedColumns { get; } = [];
  public EntityState EntityState { get; private set; }
  public (string userId, string userName) ByUser { get; private set; }
  public long? PkValue { get; private set; }
  public string? PkValueString { get; set; }
  
  public DateTime Created { get; set; }

  public AuditEntryItem(string tableName, string? schemaName, object pkValue, EntityState entityState)
  {
    TableName = tableName;
    SchemaName = schemaName;
    EntityState = entityState;
   // ByUser = auditUserProvider.GetUser();
    SetPK(pkValue);
  }

  public void AddEntry(string columnName, object? oldValue, object? newValue)
  {
    ChangedColumns.Add(new AuditEntryColumnItem(columnName, oldValue, newValue));
  }

  public void SetUser((string userId, string userName) user)
  {
    ByUser = user;
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
}