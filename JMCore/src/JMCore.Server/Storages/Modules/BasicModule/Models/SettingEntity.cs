using System.ComponentModel.DataAnnotations;
using JMCore.Server.Storages.Base.Audit.Configuration;

namespace JMCore.Server.Storages.Modules.BasicModule.Models;

[Auditable]
public class SettingEntity
{
  public int Id { get; set; }
  
  [MaxLength(1024)]
  public string Key { get; set; } = null!;
  [MaxLength(1024)]
  public string Value { get; set; } = null!;
  public bool? IsSystem { get; set; }
}