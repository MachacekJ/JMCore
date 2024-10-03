using ACore.Extensions;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using Mapster;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKLong.Models;

public class TestPKLongData
{
  public long Id { get; set; }
  public string Name { get; set; }
  public string NotAuditableColumn { get; set; }
  public DateTime Created { get; set; }

  internal static TestPKLongData Create(TestPKLongEntity entity)
    => entity.Adapt<TestPKLongData>();
}