using ACore.Extensions;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.Mongo.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Models;

public class TestAuditData<T>
{
  public T Id { get; set; } = default(T) ?? throw new Exception($"Cannot create {nameof(Id)} for type {typeof(T).Name}");
  public string Name { get; set; } = string.Empty;
  public string NotAuditableColumn { get; set; } = string.Empty;
  
  public DateTime Created { get; set; }
  
  internal static TestAuditData<T> Create<T>(TestAuditEntity entity)
  {
    var d = new TestAuditData<T>();
    d.CopyPropertiesFrom(entity);
    return d;
  }
  
  internal static TestAuditData<T> Create<T>(TestAttributeAuditPKMongoEntity entity)
  {
    var d = new TestAuditData<T>();
    d.CopyPropertiesFrom(entity);
    return d;
  }
}