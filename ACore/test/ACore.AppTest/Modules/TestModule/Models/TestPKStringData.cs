using ACore.AppTest.Modules.TestModule.Storages.EF.Models;
using ACore.Extensions;
using ACore.Server.Storages.Models;

namespace ACore.AppTest.Modules.TestModule.Models;

public class TestPKStringData
{
  public string Id { get; set; } = string.Empty;
  public string Name { get; set; } = null!;
  
  internal static TestPKStringData Create(TestPKStringEntity entity)
  {
    var testPKGuidData = new TestPKStringData();
    testPKGuidData.CopyPropertiesFrom(entity);
    return testPKGuidData;
  }
}