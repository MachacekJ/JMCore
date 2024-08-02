using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACore.Server.Modules.AuditModule.Storage.Models;

public class AuditColumnEntity
{
  [Key]
  public int Id { get; set; }

  public int AuditTableId { get; set; }

  [MaxLength(255)]
  public string ColumnName { get; set; } = null!;

  [ForeignKey("AuditTableId")]
  public AuditTableEntity AuditTable { get; set; } = null!;
}