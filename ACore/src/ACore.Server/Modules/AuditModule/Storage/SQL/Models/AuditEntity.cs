using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

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
  public AuditTableEntity AuditTable { get; set; }

  [ForeignKey("AuditUserId")]
  public AuditUserEntity User { get; set; }

  public ICollection<AuditValueEntity> AuditValues { get; set; }
}