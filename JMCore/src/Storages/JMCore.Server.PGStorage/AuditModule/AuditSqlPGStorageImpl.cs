using JMCore.Server.Modules.AuditModule.Storage;
using JMCore.Server.Modules.AuditModule.Storage.Models;
using JMCore.Server.Storages.EF;
using JMCore.Server.Storages.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JMCore.Server.PGStorage.AuditModule;

public class AuditSqlPGStorageImpl(DbContextOptions<AuditSqlPGStorageImpl> options, IMediator mediator, ILogger<AuditSqlStorageImpl> logger) : AuditSqlStorageImpl(options, mediator, logger)
{
  public override DbScriptBase UpdateScripts => new Scripts.ScriptRegistrations();
  protected override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Postgres);
  
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<AuditColumnEntity>().ToTable("audit_column");
    modelBuilder.Entity<AuditColumnEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<AuditColumnEntity>().Property(t => t.Id).HasColumnName("audit_column_id");
    modelBuilder.Entity<AuditColumnEntity>().Property(t => t.AuditTableId).HasColumnName("audit_table_id");
    modelBuilder.Entity<AuditColumnEntity>().Property(t => t.ColumnName).HasColumnName("column_name");
   
    
    modelBuilder.Entity<AuditEntity>().ToTable("audit");
    modelBuilder.Entity<AuditEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<AuditEntity>().Property(t => t.Id).HasColumnName("audit_id");
    modelBuilder.Entity<AuditEntity>().Property(t => t.AuditTableId).HasColumnName("audit_table_id");
    modelBuilder.Entity<AuditEntity>().Property(t => t.PKValue).HasColumnName("pk_value");
    modelBuilder.Entity<AuditEntity>().Property(t => t.PKValueString).HasColumnName("pk_value_string");
    modelBuilder.Entity<AuditEntity>().Property(t => t.AuditUserId).HasColumnName("audit_user_id");
    modelBuilder.Entity<AuditEntity>().Property(t => t.DateTime).HasColumnName("date_time");
    modelBuilder.Entity<AuditEntity>().Property(t => t.EntityState).HasColumnName("entity_state");
    
    modelBuilder.Entity<AuditTableEntity>().ToTable("audit_table");
    modelBuilder.Entity<AuditTableEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<AuditTableEntity>().Property(t => t.Id).HasColumnName("audit_table_id");
    modelBuilder.Entity<AuditTableEntity>().Property(t => t.TableName).HasColumnName("table_name");
    modelBuilder.Entity<AuditTableEntity>().Property(t => t.SchemaName).HasColumnName("schema_name");

    modelBuilder.Entity<AuditUserEntity>().ToTable("audit_user");
    modelBuilder.Entity<AuditUserEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<AuditUserEntity>().Property(t => t.Id).HasColumnName("audit_user_id");
    modelBuilder.Entity<AuditUserEntity>().Property(t => t.UserId).HasColumnName("user_id");
    modelBuilder.Entity<AuditUserEntity>().Property(t => t.UserName).HasColumnName("user_name");
    
    modelBuilder.Entity<AuditValueEntity>().ToTable("audit_value");
    modelBuilder.Entity<AuditValueEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<AuditValueEntity>().Property(t => t.Id).HasColumnName("audit_value_id");
    modelBuilder.Entity<AuditValueEntity>().Property(t => t.AuditId).HasColumnName("audit_id");
    modelBuilder.Entity<AuditValueEntity>().Property(t => t.AuditColumnId).HasColumnName("audit_column_id");
    modelBuilder.Entity<AuditValueEntity>().Property(t => t.OldValueString).HasColumnName("old_value_string");
    modelBuilder.Entity<AuditValueEntity>().Property(t => t.NewValueString).HasColumnName("new_value_string");
    modelBuilder.Entity<AuditValueEntity>().Property(t => t.OldValueInt).HasColumnName("old_value_int");
    modelBuilder.Entity<AuditValueEntity>().Property(t => t.NewValueInt).HasColumnName("new_value_int");
    modelBuilder.Entity<AuditValueEntity>().Property(t => t.OldValueLong).HasColumnName("old_value_long");
    modelBuilder.Entity<AuditValueEntity>().Property(t => t.NewValueLong).HasColumnName("new_value_long");
    modelBuilder.Entity<AuditValueEntity>().Property(t => t.OldValueBool).HasColumnName("old_value_bool");
    modelBuilder.Entity<AuditValueEntity>().Property(t => t.NewValueBool).HasColumnName("new_value_bool");
    modelBuilder.Entity<AuditValueEntity>().Property(t => t.OldValueGuid).HasColumnName("old_value_guid");
    modelBuilder.Entity<AuditValueEntity>().Property(t => t.NewValueGuid).HasColumnName("new_value_guid");
    
  }
}