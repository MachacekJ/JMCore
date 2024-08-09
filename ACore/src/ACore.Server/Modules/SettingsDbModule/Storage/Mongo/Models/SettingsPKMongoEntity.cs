using System.ComponentModel.DataAnnotations;
using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Storages.Definitions.Models.PK;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace ACore.Server.Modules.SettingsDbModule.Storage.Mongo.Models;

[Auditable(1)]
public class SettingsPKMongoEntity : PKMongoEntity
{
  [MaxLength(1024)]
  public string Key { get; set; }
  
  [MaxLength(1024)]
  public string Value { get; set; }
  
  public bool? IsSystem { get; set; }
}