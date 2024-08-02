// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ACore.Tests.Implementations.Modules.TestModule.Storages.Models;

public class TestManualAuditEntity
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;
  public string NotAuditableColumn { get; set; } = null!;
  public DateTime Created { get; set; }
}