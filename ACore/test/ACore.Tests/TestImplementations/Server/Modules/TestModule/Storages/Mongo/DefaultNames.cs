using System.Linq.Expressions;
using ACore.Server.Storages.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.Mongo.Models;

#pragma warning disable CS8603 // Possible null reference return.

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.Mongo;

public static class DefaultNames
{
  public static Dictionary<string, StorageEntityNameDefinition> ObjectNameMapping => new()
  {
    //  { nameof(TestEntity), new StorageEntityNameDefinition("test", TestEntityColumnNames) },
    { nameof(TestAttributeAuditPKMongoEntity), new StorageEntityNameDefinition("testAttribute", TestAttributeAuditEntityColumnNames) },
    //   { nameof(TestManualAuditEntity), new StorageEntityNameDefinition("test_manual_audit", TestManualAuditEntityColumnNames) },
    //  { nameof(TestValueTypeEntity), new StorageEntityNameDefinition("test_value_type", TestValueTypeEntityColumnNames) },
    //   { nameof(TestPKGuidEntity), new StorageEntityNameDefinition("test_pk_guid", TestPKGuidEntityColumnNames) },
    //   { nameof(TestPKStringEntity), new StorageEntityNameDefinition("test_pk_string", TestPKStringEntityColumnNames) }
  };
  
  private static Dictionary<Expression<Func<TestAttributeAuditPKMongoEntity, object>>, string> TestAttributeAuditEntityColumnNames => new()
  {
    { e => e.Id, "_id" },
    { e => e.Name, "name" },
    { e => e.NotAuditableColumn, "notAuditableColumn" },
    { e => e.Created, "created" }
  };
  
}