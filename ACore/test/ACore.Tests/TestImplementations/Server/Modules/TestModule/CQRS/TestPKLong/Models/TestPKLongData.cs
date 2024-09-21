using ACore.Extensions;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKLong.Models;

public class TestPKLongData
{
  public long Id { get; set; }
  public string Name { get; set; } = null!;
  public string NotAuditableColumn { get; set; } = null!;
  public DateTime Created { get; set; }

  internal static TestPKLongData Create(TestPKLongEntity entity)
  {
    var testPKGuidData = new TestPKLongData();
    testPKGuidData.CopyPropertiesFrom(entity);
    return testPKGuidData;
  }
}