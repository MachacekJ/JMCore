using System.Linq.Expressions;
using ACore.Server.Modules.SettingModule.Storage.Mongo.Models;
using ACore.Server.Storages.Models;

namespace ACore.Server.Modules.SettingModule.Storage.Mongo;
#pragma warning disable CS8603 // Possible null reference return.

public static class CollectionNames
{
  public static Dictionary<string, StorageEntityNameDefinition> ObjectNameMapping => new()
  {
    { nameof(SettingMongoEntity), new StorageEntityNameDefinition("setting", SettingMongoEntityColumnNames) },
  };
  
  private static Dictionary<Expression<Func<SettingMongoEntity, object>>, string> SettingMongoEntityColumnNames => new()
  {
    { e => e.Id, "_id" },
    { e => e.Key, "key" },
    { e => e.Value, "value" },
    { e => e.IsSystem, "isSystem" }
  };
}