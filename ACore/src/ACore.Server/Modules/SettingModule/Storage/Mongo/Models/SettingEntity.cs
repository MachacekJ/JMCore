using System.ComponentModel.DataAnnotations;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Storages.Models;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace ACore.Server.Modules.SettingModule.Storage.Mongo.Models;

[Auditable]
public class SettingMongoEntity : MongoStorageEntity
{
  [MaxLength(1024)]
  public string Key { get; set; }
  [MaxLength(1024)]
  public string Value { get; set; }
  public bool? IsSystem { get; set; }
}