using System.ComponentModel.DataAnnotations;
using ACore.Server.Modules.AuditModule.Configuration;

namespace ACore.Tests.Implementations.Modules.TestModule.Storages.Models;

// ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
[Auditable]
public class TestPKGuidEntity
{
  [Key]
  public Guid Id { get; set; }
  public string Name { get; set; } = null!;
}