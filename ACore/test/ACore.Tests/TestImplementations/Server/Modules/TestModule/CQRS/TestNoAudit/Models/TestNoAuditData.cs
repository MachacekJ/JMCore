using ACore.Extensions;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestNoAudit.Models;

public class TestNoAuditData(string name)
{
  public string UId { get; set; } = Guid.NewGuid().ToString();
  public int Id { get; set; }

  public string Name { get; set; } = name;

  public DateTime Created { get; set; }

  internal static KeyValuePair<string, TestNoAuditData> Create(TestNoAuditEntity noAuditEntity, string saltForHash)
  {
    var testPKGuidData = new TestNoAuditData(noAuditEntity.Name);
    testPKGuidData.CopyPropertiesFrom(noAuditEntity);
    return new KeyValuePair<string, TestNoAuditData>(noAuditEntity.HashObject(saltForHash), testPKGuidData);
  }
}