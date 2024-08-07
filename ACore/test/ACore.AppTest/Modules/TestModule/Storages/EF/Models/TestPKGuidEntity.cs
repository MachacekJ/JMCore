using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Storages.Models;

namespace ACore.AppTest.Modules.TestModule.Storages.EF.Models;

// ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
[Auditable]
internal class TestPKGuidEntity : GuidStorageEntity
{
  public string Name { get; set; } = null!;
}