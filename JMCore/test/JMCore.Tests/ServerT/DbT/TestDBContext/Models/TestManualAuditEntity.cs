using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace JMCore.Tests.ServerT.DbT.TestDBContext.Models;

[Table("TestManualAudit")]
public class TestManualAuditEntity
{
  [Key]
  [Column("test_manual_audit_id")]
  public int Id { get; set; }
  
  [Column("name")]
  public string Name { get; set; } = null!;
  
  [Column("not_auditable_column")]
  public string NotAuditableColumn { get; set; } = null!;
  
  [Column("created")]
  public DateTime Created { get; set; }
}