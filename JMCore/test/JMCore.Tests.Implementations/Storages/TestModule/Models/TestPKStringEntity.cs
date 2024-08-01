using System.ComponentModel.DataAnnotations;
using JMCore.Server.Modules.AuditModule.Configuration;

namespace JMCore.Tests.Implementations.Storages.TestModule.Models;

[Auditable]
public class TestPKStringEntity
{
  [Key]
  [MaxLength(50)]
  public string Id { get; set; } = null!;
  
  [MaxLength(20)]
  public string Name { get; set; } = null!;
}