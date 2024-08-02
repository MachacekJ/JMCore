

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ACore.AppTest.Modules.TestModule.Storages.Models;

internal class TestEntity
{
  public string UId { get; set; } = Guid.NewGuid().ToString();
  public int Id { get; set; }

  public string Name { get; set; } = null!;

  public DateTime Created { get; set; }
}