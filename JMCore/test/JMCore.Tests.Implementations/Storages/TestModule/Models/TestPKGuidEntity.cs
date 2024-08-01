using System.ComponentModel.DataAnnotations;
using JMCore.Server.Modules.AuditModule.Configuration;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace JMCore.Tests.Implementations.Storages.TestModule.Models;

[Auditable]
public class TestPKGuidEntity
{
  [Key]
  public Guid Id { get; set; }
  public string Name { get; set; } = null!;
}