using ACore.AppTest.Modules.TestModule.Storages.EF.Models;
using ACore.Extensions;

namespace ACore.AppTest.Modules.TestModule.Models;

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