using ACore.AppTest.Modules.TestModule.Storages.SQL.Models;
using ACore.Extensions;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestPKString.Models;

public class TestPKStringData
{
  public string Id { get; set; } = string.Empty;
  public string Name { get; set; } = null!;
  
  internal static TestPKStringData Create(TestPKPKStringEntity entity)
  {
    var testPKGuidData = new TestPKStringData();
    testPKGuidData.CopyPropertiesFrom(entity);
    return testPKGuidData;
  }
}

public static class TestPKStringDataExtensions
{
  internal static TestPKPKStringEntity ToEntity(this TestPKStringData data)
  {
    var en = new TestPKPKStringEntity
    {
      Name = string.Empty
    };
    en.CopyPropertiesFrom(data);
    return en;
  }
}