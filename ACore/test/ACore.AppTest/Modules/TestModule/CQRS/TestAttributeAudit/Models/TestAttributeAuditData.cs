using ACore.AppTest.Modules.TestModule.Storages.Mongo.Models;
using ACore.AppTest.Modules.TestModule.Storages.SQL.Models;
using ACore.Extensions;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit.Models;

public class TestAttributeAuditData<T>
{
  public T Id { get; set; } = default(T) ?? throw new Exception("Unkofdsa");
  public string Name { get; set; } = string.Empty;
  public string NotAuditableColumn { get; set; } = null!;
  public DateTime Created { get; set; }
  
  internal static TestAttributeAuditData<T> Create<T>(TestAttributeAuditEntity entity)
  {
    var d = new TestAttributeAuditData<T>();
    d.CopyPropertiesFrom(entity);
    return d;
  }
  
  internal static TestAttributeAuditData<T> Create<T>(TestAttributeAuditPKMongoEntity entity)
  {
    var d = new TestAttributeAuditData<T>();
    d.CopyPropertiesFrom(entity);
    return d;
  }
}