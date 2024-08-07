

// ReSharper disable UnusedAutoPropertyAccessor.Global

using ACore.Server.Storages.Models;

namespace ACore.AppTest.Modules.TestModule.Storages.EF.Models;

internal class TestEntity : IntStorageEntity
{
  public Guid UId { get; set; } = Guid.Empty;
  
  public string Name { get; set; } = null!;

  public DateTime Created { get; set; }
}