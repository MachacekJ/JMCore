using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JMCore.Server.DB.DbContexts.AuditStructure.Models;

[Table("audit_column")]
public class AuditColumnEntity
{
  [Key]
  [Column("audit_column_id")]
  public int Id { get; set; }

  [Column("audit_table_id")]
  public int AuditTableId { get; set; }

  [Column("column_name")]
  [MaxLength(255)]
  public string ColumnName { get; set; } = null!;

  [ForeignKey("AuditTableId")]
  public AuditTableEntity AuditTable { get; set; } = null!;
}