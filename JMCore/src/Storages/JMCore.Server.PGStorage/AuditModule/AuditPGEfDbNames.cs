using System.Linq.Expressions;
using JMCore.Server.Modules.AuditModule.Storage.Models;
using JMCore.Server.Storages.Models;

namespace JMCore.Server.PGStorage.AuditModule;

public static class AuditPGEfDbNames
{
  public static Dictionary<Type, StorageEntityNameDefinition> ObjectNameMapping => new()
  {
    { typeof(AuditColumnEntity), new StorageEntityNameDefinition("audit_column", AuditColumnEntityColumnNames) },
    { typeof(AuditEntity), new StorageEntityNameDefinition("audit", AuditEntityColumnNames) },
    { typeof(AuditTableEntity), new StorageEntityNameDefinition("audit_table", AuditTableEntityColumnNames) },
    { typeof(AuditUserEntity), new StorageEntityNameDefinition("audit_user", AuditUserEntityColumnNames) },
    { typeof(AuditValueEntity), new StorageEntityNameDefinition("audit_value", AuditValueColumnNames) },
  };
  
  private static Dictionary<Expression<Func<AuditColumnEntity, object>>, string> AuditColumnEntityColumnNames => new()
  {
    { e => e.Id, "audit_column_id" },
    { e => e.AuditTableId, "audit_table_id" },
    { e => e.ColumnName, "column_name" }
  };
  
  private static Dictionary<Expression<Func<AuditEntity, object>>, string> AuditEntityColumnNames => new()
  {
    { e => e.Id, "audit_id" },
    { e => e.AuditTableId, "audit_table_id" },
    { e => e.PKValue!, "pk_value" },
    { e => e.PKValueString!, "pk_value_string" },
    { e => e.AuditUserId!, "audit_user_id" },
    { e => e.DateTime, "date_time" },
    { e => e.EntityState, "entity_state" }
  };
  
  private static Dictionary<Expression<Func<AuditTableEntity, object>>, string> AuditTableEntityColumnNames => new()
  {
    { e => e.Id, "audit_table_id" },
    { e => e.TableName, "table_name" },
    { e => e.SchemaName!, "schema_name" }
  };
  
  private static Dictionary<Expression<Func<AuditUserEntity, object>>, string> AuditUserEntityColumnNames => new()
  {
    { e => e.Id, "audit_user_id" },
    { e => e.UserId, "user_id" },
    { e => e.UserName!, "user_name" }
  };
  
  private static Dictionary<Expression<Func<AuditValueEntity, object>>, string> AuditValueColumnNames => new()
  {
    { e => e.Id, "audit_value_id" },
    { e => e.AuditId, "audit_id" },
    { e => e.AuditColumnId, "audit_column_id" },
    { e => e.OldValueString!, "old_value_string" },
    { e => e.NewValueString!, "new_value_string" },
    { e => e.OldValueInt!, "old_value_int" },
    { e => e.NewValueInt!, "new_value_int" },
    { e => e.OldValueLong!, "old_value_long" },
    { e => e.NewValueLong!, "new_value_long" },
    { e => e.OldValueBool!, "old_value_bool" },
    { e => e.NewValueBool!, "new_value_bool" },
    { e => e.OldValueGuid!, "old_value_guid" },
    { e => e.NewValueGuid!, "new_value_guid" }
  };
}

