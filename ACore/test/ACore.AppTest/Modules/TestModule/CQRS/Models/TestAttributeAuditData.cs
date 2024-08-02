using ACore.AppTest.Modules.TestModule.Storages.Models;
using ACore.Extensions;

namespace ACore.AppTest.Modules.TestModule.CQRS.Models;

public class TestAttributeAuditData
{
  /// <summary>
  /// For the purposes of mongodb.
  /// </summary>
  public string UId { get; set; } = Guid.NewGuid().ToString();
  
  /// <summary>
  /// For the purposes of Postgres.
  /// </summary>
  public int Id { get; set; }
  public string Name { get; set; } = null!;
  public string NotAuditableColumn { get; set; } = null!;
  public DateTime Created { get; set; }
  
  internal static TestAttributeAuditData Create(TestAttributeAuditEntity entity)
  {
    var testPKGuidData = new TestAttributeAuditData();
    testPKGuidData.CopyPropertiesFrom(entity);
    return testPKGuidData;
  }
}