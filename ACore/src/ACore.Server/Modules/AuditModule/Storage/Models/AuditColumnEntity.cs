using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACore.Server.Modules.AuditModule.Storage.Models;

public class AuditColumnEntity
{
  [Key]
  public int Id { get; set; }

  [Required]
  public int AuditTableId { get; set; }

  [MaxLength(255)]
  [Required]
  public string ColumnName { get; set; } = string.Empty;

  [MaxLength(1024)]
  [Required]
  public string DataType { get; set; } = string.Empty;
  
  [ForeignKey("AuditTableId")]
  public AuditTableEntity? AuditTable { get; set; }
}