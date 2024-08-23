using System.Linq.Expressions;
using ACore.Server.Modules.SettingModule.Storage.SQL.Models;
using ACore.Server.Storages.Models;

#pragma warning disable CS8603 // Possible null reference return.

namespace ACore.Server.Modules.SettingModule.Storage.SQL.PG;

public static class DefaultNames
{
  public static Dictionary<string, StorageEntityNameDefinition> ObjectNameMapping => new()
  {
    { nameof(SettingEntity), new StorageEntityNameDefinition("setting", TestEntityColumnNames) },
  };

  private static Dictionary<Expression<Func<SettingEntity, object>>, string> TestEntityColumnNames => new()
  {
    { e => e.Id, "setting_id" },
    { e => e.Key, "key" },
    { e => e.Value, "value" },
    { e => e.IsSystem, "is_system" }
  };

}