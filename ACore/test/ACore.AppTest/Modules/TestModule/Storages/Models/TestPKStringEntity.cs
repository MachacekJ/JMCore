using System.ComponentModel.DataAnnotations;
using ACore.Server.Modules.AuditModule.Configuration;

namespace ACore.AppTest.Modules.TestModule.Storages.Models;

[Auditable]
internal class TestPKStringEntity
{
  [Key]
  [MaxLength(50)]
  public string Id { get; set; } = null!;
  
  [MaxLength(20)]
  public string Name { get; set; } = null!;
}