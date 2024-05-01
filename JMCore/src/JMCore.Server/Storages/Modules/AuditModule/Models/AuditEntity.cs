using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JMCore.Server.Storages.Modules.AuditModule.Models;

[Table("audit")]
public class AuditEntity
{
  [Key]
  [Column("audit_id")]
  public long Id { get; set; }

  [Column("audit_table_id")]
  public int AuditTableId { get; set; }

  [Column("pk_value")]
  public long? PKValue { get; set; }

  [Column("pk_value_string")]
  [MaxLength(450)]
  public string? PKValueString { get; set; }

  [Column("audit_user_id")]
  public int? AuditUserId { get; set; }

  [Column("date_time")]
  public DateTime DateTime { get; set; }

  [Column("entity_state")]
  public EntityState EntityState { get; set; }

  [ForeignKey("AuditTableId")]
  public AuditTableEntity AuditTable { get; set; } = null!;

  [ForeignKey("AuditUserId")]
  public AuditUserEntity User { get; set; } = null!;

  public ICollection<AuditValueEntity> AuditValues { get; set; } = null!;
}