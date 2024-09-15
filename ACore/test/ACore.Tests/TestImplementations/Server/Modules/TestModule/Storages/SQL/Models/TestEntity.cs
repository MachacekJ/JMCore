using System.ComponentModel.DataAnnotations;
using ACore.Extensions;
using ACore.Server.Storages.Models.PK;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.Test.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

internal class TestEntity : PKIntEntity
{
  [MaxLength(200)]
  public string Name { get; set; } = string.Empty;

  public DateTime Created { get; set; }

  public static TestEntity Create(TestData data)
  {
    var en = new TestEntity();
    en.CopyPropertiesFrom(data);
    return en;
  }
}
