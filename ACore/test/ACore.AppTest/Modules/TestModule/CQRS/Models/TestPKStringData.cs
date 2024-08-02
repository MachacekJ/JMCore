using ACore.AppTest.Modules.TestModule.Storages.Models;
using ACore.Extensions;

namespace ACore.AppTest.Modules.TestModule.CQRS.Models;

public class TestPKStringData
{
  public string Id { get; set; } = null!;
  public string Name { get; set; } = null!;
  
  internal static TestPKStringData Create(TestPKStringEntity entity)
  {
    var testPKGuidData = new TestPKStringData();
    testPKGuidData.CopyPropertiesFrom(entity);
    return testPKGuidData;
  }
}