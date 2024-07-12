using System.ComponentModel.DataAnnotations;
using JMCore.Server.Storages.Base.Audit.Configuration;

namespace JMCore.Tests.ServerT.StoragesT.Implemantations.TestStorageModule.Models;

[Auditable]
public class TestPKStringEntity
{
  [Key]
  [MaxLength(50)]
  public string Id { get; set; } = null!;
  
  [MaxLength(20)]
  public string Name { get; set; } = null!;
}