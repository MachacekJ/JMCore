using ACore.AppTest.Modules.TestModule.Storages.Models;
using ACore.Extensions;

namespace ACore.AppTest.Modules.TestModule.CQRS.Models;

public class TestData
{
  public string UId { get; set; } = Guid.NewGuid().ToString();
  public int Id { get; set; }

  public string Name { get; set; } = null!;

  public DateTime Created { get; set; }
  
  internal static TestData Create(TestEntity entity)
  {
    var testPKGuidData = new TestData();
    testPKGuidData.CopyPropertiesFrom(entity);
    return testPKGuidData;
  }
}