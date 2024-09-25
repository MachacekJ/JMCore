using System.ComponentModel.DataAnnotations;
using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Storages.Models.PK;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace ACore.Server.Modules.SettingsDbModule.Storage.SQL.Models;

[Auditable(1)]
public class SettingsEntity: PKIntEntity
{
  [MaxLength(256)]
  public string Key { get; set; }
  [MaxLength(65536)]
  public string Value { get; set; }
  public bool? IsSystem { get; set; }
}