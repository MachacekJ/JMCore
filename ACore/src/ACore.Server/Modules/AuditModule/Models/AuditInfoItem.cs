using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace ACore.Server.Modules.AuditModule.Models;

public class AuditInfoItem
{
  public bool IsAuditable { get; set; }
  public string TableName { get; }
  public string? SchemaName { get; }
  public int Version { get; }
  public List<AuditInfoColumnItem> Columns { get; set; } = [];
  public EntityState EntityState { get; private set; }
  public string UserId { get; private set; }
  public long? PkValue { get; private set; }
  public string? PkValueString { get; set; }
  public DateTime Created { get; set; }

  public AuditInfoItem(string tableName, string? schemaName, int version, object pkValue, EntityState entityState, string userId)
  {
    IsAuditable = false;
    TableName = tableName;
    SchemaName = schemaName;
    EntityState = entityState;
    Version = version;
    UserId = userId;
    SetPK(pkValue);
  }

  public void AddColumnEntry(AuditInfoColumnItem columnItem)
  => Columns.Add(columnItem);
  

  public void SetEntityState(EntityState entityState)
  {
    EntityState = entityState;
  }

  private void SetPK<TPK>(TPK pkValue)
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

    if (PkValueString == null)
      return default;

    if (typeof(TPK) == typeof(Guid))
    {
      var g = new Guid(PkValueString);
      return (TPK)Convert.ChangeType(g, typeof(TPK));
    }

    if (typeof(TPK) == typeof(ObjectId))
    {
      var g = new ObjectId(PkValueString);
      return (TPK)Convert.ChangeType(g, typeof(TPK));
    }

    return (TPK)Convert.ChangeType(PkValueString, typeof(TPK));
  }
}