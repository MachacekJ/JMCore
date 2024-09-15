using ACore.Extensions;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Storages.Models.PK;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKGuid.Models;

// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

[Auditable(1)]
internal class TestPKGuidEntity : PKGuidEntity
{
  public string? Name { get; set; }
  
  public static TestPKGuidEntity Create(TestPKGuidData data)
  {
    var en = new TestPKGuidEntity();
    en.CopyPropertiesFrom(data);
    return en;
  }
}