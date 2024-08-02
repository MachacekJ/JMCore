using System.ComponentModel.DataAnnotations;
using ACore.Server.Modules.AuditModule.Configuration;

namespace ACore.AppTest.Modules.TestModule.Storages.Models;

// ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
[Auditable]
internal class TestPKGuidEntity
{
  [Key]
  public Guid Id { get; set; }
  public string Name { get; set; } = null!;
}