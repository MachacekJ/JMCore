using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JMCore.Server.Storages.Audit;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace JMCore.Tests.ServerT.DbT.TestDBContext.Models;

[Auditable]
[Table("test_attribute_audit")]
public class TestAttributeAuditEntity
{
  [Key]
  [Column("test_attribute_audit_id")]
  public int Id { get; set; }

  [Column("name")]
  [MaxLength(50)]
  public string Name { get; set; } = null!;

  [NotAuditable]
  [Column("not_auditable_column")]
  [MaxLength(50)]
  public string NotAuditableColumn { get; set; } = null!;
  
  [Column("created")]
  public DateTime Created { get; set; }
}