using System.ComponentModel.DataAnnotations;
using JMCore.Server.Modules.AuditModule.Configuration;

namespace JMCore.Server.Modules.SettingModule.Storage.Models;

[Auditable]
public class SettingEntity
{
  // [MaxLength(36)]
  // public string UId { get; set; } = Guid.NewGuid().ToString();
  
  public int Id { get; set; }
  
  [MaxLength(1024)]
  public string Key { get; set; } = null!;
  [MaxLength(1024)]
  public string Value { get; set; } = null!;
  public bool? IsSystem { get; set; }
}