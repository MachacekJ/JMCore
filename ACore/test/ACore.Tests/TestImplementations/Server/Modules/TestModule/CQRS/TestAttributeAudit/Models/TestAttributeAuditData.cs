using ACore.Extensions;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.Mongo.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAttributeAudit.Models;

public class TestAttributeAuditData<T>
{
  public T Id { get; set; } = default(T) ?? throw new Exception($"Cannot create {nameof(Id)} for type {typeof(T).Name}");
  public string Name { get; set; } = string.Empty;
  public string NotAuditableColumn { get; set; } = null!;
  public DateTime Created { get; set; }
  
  internal static TestAttributeAuditData<T> Create<T>(TestAttributeAuditPKIntEntity pkIntEntity)
  {
    var d = new TestAttributeAuditData<T>();
    d.CopyPropertiesFrom(pkIntEntity);
    return d;
  }
  
  internal static TestAttributeAuditData<T> Create<T>(TestAttributeAuditPKMongoEntity entity)
  {
    var d = new TestAttributeAuditData<T>();
    d.CopyPropertiesFrom(entity);
    return d;
  }
}