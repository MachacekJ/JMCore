// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace JMCore.Tests.Implementations.Modules.TestModule.Storages.Models;

public class TestManualAuditEntity
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;
  public string NotAuditableColumn { get; set; } = null!;
  public DateTime Created { get; set; }
}