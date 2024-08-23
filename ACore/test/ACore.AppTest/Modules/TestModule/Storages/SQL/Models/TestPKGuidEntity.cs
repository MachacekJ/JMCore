using ACore.AppTest.Modules.TestModule.CQRS.TestPKGuid.Models;
using ACore.Extensions;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Storages.Models;
using ACore.Server.Storages.Models.PK;

namespace ACore.AppTest.Modules.TestModule.Storages.SQL.Models;

// ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
// ReSharper disable once PropertyCanBeMadeInitOnly.Global
[Auditable]
internal class TestPKGuidEntity : PKGuidEntity
{
  public string Name { get; set; } = string.Empty;
}

public static class TestPKGuidEntityExtensions
{
  internal static TestPKGuidEntity ToEntity(this TestPKGuidData data)
  {
    var en = new TestPKGuidEntity
    {
      Name = string.Empty
    };
    en.CopyPropertiesFrom(data);
    return en;
  }
}