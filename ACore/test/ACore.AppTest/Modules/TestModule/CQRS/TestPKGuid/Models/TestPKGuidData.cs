using ACore.AppTest.Modules.TestModule.Storages.SQL.Models;
using ACore.Extensions;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestPKGuid.Models;

public class TestPKGuidData
{
  public Guid Id { get; set; } = Guid.Empty;
  public string Name { get; set; } = null!;
  
  internal static TestPKGuidData Create(TestPKGuidEntity entity)
  {
    var testPKGuidData = new TestPKGuidData();
    testPKGuidData.CopyPropertiesFrom(entity);
    return testPKGuidData;
  }
}