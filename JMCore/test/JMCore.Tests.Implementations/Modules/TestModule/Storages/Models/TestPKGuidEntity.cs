using System.ComponentModel.DataAnnotations;
using JMCore.Server.Modules.AuditModule.Configuration;

namespace JMCore.Tests.Implementations.Modules.TestModule.Storages.Models;

// ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
[Auditable]
public class TestPKGuidEntity
{
  [Key]
  public Guid Id { get; set; }
  public string Name { get; set; } = null!;
}