using ACore.Extensions;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.Test.Models;

public class TestData(string name)
{
  public string UId { get; set; } = Guid.NewGuid().ToString();
  public int Id { get; set; }

  public string Name { get; set; } = name;

  public DateTime Created { get; set; }

  internal static TestData Create(TestEntity entity)
  {
    var testPKGuidData = new TestData(entity.Name);
    testPKGuidData.CopyPropertiesFrom(entity);
    return testPKGuidData;
  }
}