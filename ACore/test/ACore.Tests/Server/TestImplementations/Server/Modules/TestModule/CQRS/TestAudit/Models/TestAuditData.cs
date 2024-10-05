using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.Mongo.Models;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using Mapster;

namespace ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Models;

public class TestAuditData<T>
{
  public T Id { get; set; } = default(T) ?? throw new Exception($"Cannot create {nameof(Id)} for type {typeof(T).Name}");
  public string Name { get; set; } = string.Empty;

  public string? NullValue { get; set; }
  public string? NullValue2 { get; set; }
  public string? NullValue3 { get; set; }
  public string NotAuditableColumn { get; set; } = string.Empty;

  public DateTime Created { get; set; }

  internal static TestAuditData<TEntity> Create<TEntity>(TestAuditEntity entity)
    => entity.Adapt<TestAuditData<TEntity>>();

  internal static TestAuditData<TEntity> Create<TEntity>(TestPKMongoEntity entity)
    => entity.Adapt<TestAuditData<TEntity>>();
}