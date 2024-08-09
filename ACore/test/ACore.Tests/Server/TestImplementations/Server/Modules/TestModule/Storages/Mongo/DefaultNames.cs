using System.Linq.Expressions;
using ACore.Server.Storages.Definitions.Models;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.Mongo.Models;

#pragma warning disable CS8603 // Possible null reference return.

namespace ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.Mongo;

public static class DefaultNames
{
  public static Dictionary<string, StorageEntityNameDefinition> ObjectNameMapping => new()
  {
    { nameof(TestAuditEntity), new StorageEntityNameDefinition("test", TestAttributeAuditEntityColumnNames) },
  };
  
  private static Dictionary<Expression<Func<TestAuditEntity, object>>, string> TestAttributeAuditEntityColumnNames => new()
  {
    { e => e.Id, "_id" },
    { e => e.Name, "name" },
    { e => e.NotAuditableColumn, "notAuditableColumn" },
    { e => e.Created, "created" },
    { e => e.NullValue, "nullValue" },
    { e => e.NullValue2, "nullValue2" },
    { e => e.NullValue3, "nullValue3" },
  };
  
}