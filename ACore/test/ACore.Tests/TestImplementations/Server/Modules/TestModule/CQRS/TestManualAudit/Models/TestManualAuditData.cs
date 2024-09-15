using ACore.Extensions;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestManualAudit.Models;

public class TestManualAuditData
{
  public long Id { get; set; }
  public string Name { get; set; } = null!;
  public string NotAuditableColumn { get; set; } = null!;
  public DateTime Created { get; set; }

  internal static TestManualAuditData Create(TestManualAuditEntity entity)
  {
    var testPKGuidData = new TestManualAuditData();
    testPKGuidData.CopyPropertiesFrom(entity);
    return testPKGuidData;
  }
}