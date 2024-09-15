using System.Linq.Expressions;
using ACore.Server.Storages.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

#pragma warning disable CS8603 // Possible null reference return.

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.PG;

public static class DefaultNames
{
  public static Dictionary<string, StorageEntityNameDefinition> ObjectNameMapping => new()
  {
    { nameof(TestEntity), new StorageEntityNameDefinition("test", TestEntityColumnNames) },
    { nameof(TestAttributeAuditPKIntEntity), new StorageEntityNameDefinition("test_attribute_audit", TestAttributeAuditEntityColumnNames) },
  //  { nameof(TestManualAuditEntity), new StorageEntityNameDefinition("test_manual_audit", TestManualAuditEntityColumnNames) },
    { nameof(TestValueTypeEntity), new StorageEntityNameDefinition("test_value_type", TestValueTypeEntityColumnNames) },
    { nameof(TestPKGuidEntity), new StorageEntityNameDefinition("test_pk_guid", TestPKGuidEntityColumnNames) },
    { nameof(TestPKStringEntity), new StorageEntityNameDefinition("test_pk_string", TestPKStringEntityColumnNames) }
  };

  private static Dictionary<Expression<Func<TestEntity, object>>, string> TestEntityColumnNames => new()
  {
    { e => e.Id, "test_id" },
    { e => e.Name, "name" },
    { e => e.Created, "created" }
  };

  private static Dictionary<Expression<Func<TestAttributeAuditPKIntEntity, object>>, string> TestAttributeAuditEntityColumnNames => new()
  {
    { e => e.Id, "test_attribute_audit_id" },
    { e => e.Name, "name" },
    { e => e.NotAuditableColumn, "not_auditable_column" },
    { e => e.Created, "created" }
  };

  // private static Dictionary<Expression<Func<TestManualAuditEntity, object>>, string> TestManualAuditEntityColumnNames => new()
  // {
  //   { e => e.Id, "test_manual_audit_id" },
  //   { e => e.Name, "name" },
  //   { e => e.NotAuditableColumn, "not_auditable_column" },
  //   { e => e.Created, "created" }
  // };

  private static Dictionary<Expression<Func<TestValueTypeEntity, object>>, string> TestValueTypeEntityColumnNames => new()
  {
    { e => e.Id, "test_value_type_id" },
    { e => e.IntNotNull, "int_not_null" },
    { e => e.IntNull, "int_null" },
    { e => e.BigIntNotNull, "big_int_not_null" },
    { e => e.BigIntNull, "big_int_null" },
    { e => e.Bit2, "bit2" },
    { e => e.Char2, "char2" },
    { e => e.Date2, "date2" },
    { e => e.DateTime2, "datetime2" },
    { e => e.Decimal2, "decimal2" },
    { e => e.NChar2, "nchar2" },
    { e => e.NVarChar2, "nvarchar2" },
    { e => e.SmallDateTime2, "smalldatetime2" },
    { e => e.SmallInt2, "smallint2" },
    { e => e.TinyInt2, "tinyint2" },
    { e => e.Guid2, "guid2" },
    { e => e.VarBinary2, "varbinary2" },
    { e => e.VarChar2, "varchar2" },
  };

  private static Dictionary<Expression<Func<TestPKGuidEntity, object>>, string> TestPKGuidEntityColumnNames => new()
  {
    { e => e.Id, "test_pk_guid_id" },
    { e => e.Name, "name" }
  };
  
  private static Dictionary<Expression<Func<TestPKStringEntity, object>>, string> TestPKStringEntityColumnNames => new()
  {
    { e => e.Id, "test_pk_string_id" },
    { e => e.Name, "name" }
  };
}