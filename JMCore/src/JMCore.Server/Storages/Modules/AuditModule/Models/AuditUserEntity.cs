using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace JMCore.Server.Storages.Modules.AuditModule.Models;

[Table("audit_user")]
public class AuditUserEntity
{
  [Key]
  [Column("audit_user_id")]
  public int Id { get; set; }

  [Column("user_id")]
  [MaxLength(450)]
  public string UserId { get; set; } = null!;
  
  [Column("user_name")]
  [MaxLength(255)]
  public string? UserName { get; set; }
}