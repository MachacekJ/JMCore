using System.ComponentModel.DataAnnotations;
using JMCore.Server.Storages.Base.Audit.Configuration;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace JMCore.Tests.ServerT.StoragesT.Implementations.TestStorageModule.Models;

[Auditable]
public class TestAttributeAuditEntity
{
  [MaxLength(36)]
  public string UId { get; set; } = Guid.NewGuid().ToString();
  public int Id { get; set; }
  
  [MaxLength(50)]
  public string Name { get; set; } = null!;

  [NotAuditable]
  [MaxLength(50)]
  public string NotAuditableColumn { get; set; } = null!;
  
  public DateTime Created { get; set; }
}