using System.ComponentModel.DataAnnotations;
using ACore.Extensions;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKGuid.Models;

public class TestPKGuidData
{
  public Guid Id { get; set; } = Guid.Empty;
  
  [MaxLength(20)]
  public string? Name { get; set; }
  
  internal static TestPKGuidData Create(TestPKGuidEntity entity)
  {
    var testPKGuidData = new TestPKGuidData();
    testPKGuidData.CopyPropertiesFrom(entity);
    return testPKGuidData;
  }
}