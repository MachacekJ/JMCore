using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ACore.Server.Modules.AuditModule.Storage.SQL.Models;

internal class AuditEntity
{
  [Key]
  public long Id { get; set; }
  public int AuditTableId { get; set; }
  public long? PKValue { get; set; }

  [MaxLength(450)]
  public string? PKValueString { get; set; }

  public int? AuditUserId { get; set; }
  
  public DateTime DateTime { get; set; }
  
  public EntityState EntityState { get; set; }

  [ForeignKey("AuditTableId")]
  public AuditTableEntity AuditTable { get; set; } = null!;

  [ForeignKey("AuditUserId")]
  public AuditUserEntity User { get; set; } = null!;

  public ICollection<AuditValueEntity> AuditValues { get; set; } = null!;
}