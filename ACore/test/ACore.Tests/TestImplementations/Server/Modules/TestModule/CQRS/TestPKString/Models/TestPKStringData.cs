using ACore.Extensions;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKString.Models;

public class TestPKStringData
{
  public string Id { get; set; } = string.Empty;
  public string? Name { get; set; }
  
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