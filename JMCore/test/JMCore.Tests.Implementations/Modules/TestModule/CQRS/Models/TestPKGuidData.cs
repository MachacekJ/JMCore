using JMCore.Extensions;
using JMCore.Tests.Implementations.Modules.TestModule.Storages.Models;

namespace JMCore.Tests.Implementations.Modules.TestModule.CQRS.Models;

public class TestPKGuidData
{
  public Guid Id { get; set; }
  public string Name { get; set; } = null!;
  
  internal static TestPKGuidData Create(TestPKGuidEntity entity)
  {
    var testPKGuidData = new TestPKGuidData();
    testPKGuidData.CopyPropertiesFrom(entity);
    return testPKGuidData;
  }
}