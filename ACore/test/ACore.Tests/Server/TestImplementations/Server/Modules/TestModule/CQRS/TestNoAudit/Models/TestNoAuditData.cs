using ACore.Extensions;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using Mapster;

namespace ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestNoAudit.Models;

public class TestNoAuditData(string name)
{
  public string UId { get; set; } = Guid.NewGuid().ToString();
  public int Id { get; set; }

  public string Name { get; set; } = name;

  public DateTime Created { get; set; }

  internal static KeyValuePair<string, TestNoAuditData> Create(TestNoAuditEntity noAuditEntity, string saltForHash)
  {

    
    var testPKGuidData = noAuditEntity.Adapt<TestNoAuditData>();
    return new KeyValuePair<string, TestNoAuditData>(noAuditEntity.HashObject(saltForHash), testPKGuidData);
  }

  public static void MapConfig()
  {
    TypeAdapterConfig<TestNoAuditEntity, TestNoAuditData>.NewConfig()
      .ConstructUsing(src => new TestNoAuditData(src.Name));
  }
}