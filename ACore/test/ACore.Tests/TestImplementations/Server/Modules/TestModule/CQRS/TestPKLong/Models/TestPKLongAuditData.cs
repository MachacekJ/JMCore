using ACore.Extensions;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKLong.Models;

public class TestPKLongAuditData
{
  public long Id { get; set; }
  public string Name { get; set; } = null!;
  public string NotAuditableColumn { get; set; } = null!;
  public DateTime Created { get; set; }

  internal static TestPKLongAuditData Create(TestPKLongEntity entity)
  {
    var testPKGuidData = new TestPKLongAuditData();
    testPKGuidData.CopyPropertiesFrom(entity);
    return testPKGuidData;
  }
}