using System.ComponentModel.DataAnnotations;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using Mapster;

namespace ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestPKGuid.Models;

public class TestPKGuidData
{
  public Guid Id { get; set; } = Guid.Empty;
  
  [MaxLength(20)]
  public string? Name { get; set; }
  
  internal static TestPKGuidData Create(TestPKGuidEntity entity)
    => entity.Adapt<TestPKGuidData>();
}