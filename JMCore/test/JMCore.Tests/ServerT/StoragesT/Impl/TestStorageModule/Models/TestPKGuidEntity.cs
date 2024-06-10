using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JMCore.Server.Storages.Base.Audit.Configuration;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace JMCore.Tests.ServerT.StoragesT.Impl.TestStorageModule.Models;

[Auditable]
public class TestPKGuidEntity
{
  [Key]
  public Guid Id { get; set; }
  public string Name { get; set; } = null!;
}