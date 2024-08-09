using Microsoft.EntityFrameworkCore;

namespace ACore.Server.Storages.Models.SaveInfo;

public class SaveInfoItem
{
  public bool IsAuditable { get; set; }
  public string TableName { get; }
  public string? SchemaName { get; }
  public int Version { get; }
  public List<SaveInfoColumnItem> ChangedColumns { get; set; } = [];
  public EntityState EntityState { get; private set; }
  public string UserId { get; private set; }
  public long? PkValue { get; private set; }
  public string? PkValueString { get; set; }
  public DateTime Created { get; set; }

  public SaveInfoItem(bool isAuditable, string tableName, string? schemaName, int version, object pkValue, EntityState entityState, string userId)
  {
    IsAuditable = isAuditable;
    TableName = tableName;
    SchemaName = schemaName;
    EntityState = entityState;
    Version = version;
    UserId = userId;
    SetPK(pkValue);
  }

  public void AddColumnEntry(SaveInfoColumnItem columnItem)
  => ChangedColumns.Add(columnItem);
  

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