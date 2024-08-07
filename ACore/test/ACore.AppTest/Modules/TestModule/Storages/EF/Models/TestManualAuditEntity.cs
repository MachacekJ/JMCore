// ReSharper disable UnusedAutoPropertyAccessor.Global

using ACore.Server.Storages.Models;

namespace ACore.AppTest.Modules.TestModule.Storages.EF.Models;

internal class TestManualAuditEntity : LongStorageEntity
{
  public string Name { get; set; } = null!;
  public string NotAuditableColumn { get; set; } = null!;
  public DateTime Created { get; set; }
}