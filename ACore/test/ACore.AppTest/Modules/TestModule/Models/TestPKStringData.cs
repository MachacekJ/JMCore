using ACore.AppTest.Modules.TestModule.Storages.EF.Models;
using ACore.Extensions;

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

public static class TestPKStringDataExtensions
{
  internal static TestPKStringEntity ToEntity(this TestPKStringData data)
  {
    var en = new TestPKStringEntity
    {
      Name = string.Empty
    };
    en.CopyPropertiesFrom(data);
    return en;
  }
}