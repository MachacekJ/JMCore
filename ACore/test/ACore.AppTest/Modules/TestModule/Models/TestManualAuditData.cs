using ACore.AppTest.Modules.TestModule.Storages.EF.Models;
using ACore.Extensions;

namespace ACore.AppTest.Modules.TestModule.Models;

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