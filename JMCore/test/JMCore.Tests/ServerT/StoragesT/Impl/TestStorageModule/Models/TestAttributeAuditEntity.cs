using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JMCore.Server.Storages.Base.Audit.Configuration;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace JMCore.Tests.ServerT.StoragesT.Impl.TestStorageModule.Models;

[Auditable]
public class TestAttributeAuditEntity
{
  [Key]
  public int Id { get; set; }
  
  [MaxLength(50)]
  public string Name { get; set; } = null!;

  [NotAuditable]
  [MaxLength(50)]
  public string NotAuditableColumn { get; set; } = null!;
  
  public DateTime Created { get; set; }
}