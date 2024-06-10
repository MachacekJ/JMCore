using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Storages.Base.Audit.EF;
using JMCore.Server.Storages.Base.EF;
using JMCore.Tests.ServerT.StoragesT.Impl.TestStorageModule;
using JMCore.Tests.ServerT.StoragesT.Impl.TestStorageModule.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JMCore.TestsIntegrations.ServerT.StoragesT.Impl.PG.TestStorageModule;

public class TestPGEfStorageImpl(DbContextOptions<TestPGEfStorageImpl> options, IMediator mediator, ILogger<TestStorageEfContext> logger, IAuditDbService auditService) : TestStorageEfContext(options, mediator, logger, auditService)
{
  public override StorageTypeEnum StorageType => StorageTypeEnum.Postgres;
  public override DbScriptBase UpdateScripts => new Scripts.ScriptRegistrations();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<TestEntity>().ToTable("test");
    modelBuilder.Entity<TestEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<TestEntity>().Property(t => t.Id).HasColumnName("test_id");
    modelBuilder.Entity<TestEntity>().Property(t => t.Name).HasColumnName("name");
    modelBuilder.Entity<TestEntity>().Property(t => t.Created).HasColumnName("created");
    
    modelBuilder.Entity<TestAttributeAuditEntity>().ToTable("test_attribute_audit");
    modelBuilder.Entity<TestAttributeAuditEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<TestAttributeAuditEntity>().Property(t => t.Id).HasColumnName("test_attribute_audit_id");
    modelBuilder.Entity<TestAttributeAuditEntity>().Property(t => t.Name).HasColumnName("name");
    modelBuilder.Entity<TestAttributeAuditEntity>().Property(t => t.NotAuditableColumn).HasColumnName("not_auditable_column");
    modelBuilder.Entity<TestAttributeAuditEntity>().Property(t => t.Created).HasColumnName("created");
    
    modelBuilder.Entity<TestManualAuditEntity>().ToTable("test_manual_audit");
    modelBuilder.Entity<TestManualAuditEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<TestManualAuditEntity>().Property(t => t.Id).HasColumnName("test_manual_audit_id");
    modelBuilder.Entity<TestManualAuditEntity>().Property(t => t.Name).HasColumnName("name");
    modelBuilder.Entity<TestManualAuditEntity>().Property(t => t.NotAuditableColumn).HasColumnName("not_auditable_column");
    modelBuilder.Entity<TestManualAuditEntity>().Property(t => t.Created).HasColumnName("created");
    
    modelBuilder.Entity<TestValueTypeEntity>().ToTable("test_value_type");
    modelBuilder.Entity<TestValueTypeEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<TestValueTypeEntity>().Property(t => t.Id).HasColumnName("test_value_type_id");
    modelBuilder.Entity<TestValueTypeEntity>().Property(t => t.IntNotNull).HasColumnName("int_not_null");
    modelBuilder.Entity<TestValueTypeEntity>().Property(t => t.IntNull).HasColumnName("int_null");
    modelBuilder.Entity<TestValueTypeEntity>().Property(t => t.BigIntNotNull).HasColumnName("big_int_not_null");
    modelBuilder.Entity<TestValueTypeEntity>().Property(t => t.BigIntNull).HasColumnName("big_int_null");
    modelBuilder.Entity<TestValueTypeEntity>().Property(t => t.Bit2).HasColumnName("bit2");
    modelBuilder.Entity<TestValueTypeEntity>().Property(t => t.Char2).HasColumnName("char2");
    modelBuilder.Entity<TestValueTypeEntity>().Property(t => t.Date2).HasColumnName("date2");
    modelBuilder.Entity<TestValueTypeEntity>().Property(t => t.DateTime2).HasColumnName("datetime2");
    modelBuilder.Entity<TestValueTypeEntity>().Property(t => t.Decimal2).HasColumnName("decimal2");
    modelBuilder.Entity<TestValueTypeEntity>().Property(t => t.NChar2).HasColumnName("nchar2");
    modelBuilder.Entity<TestValueTypeEntity>().Property(t => t.NVarChar2).HasColumnName("nvarchar2");
    modelBuilder.Entity<TestValueTypeEntity>().Property(t => t.SmallDateTime2).HasColumnName("smalldatetime2");
    modelBuilder.Entity<TestValueTypeEntity>().Property(t => t.SmallInt2).HasColumnName("smallint2");
    modelBuilder.Entity<TestValueTypeEntity>().Property(t => t.TinyInt2).HasColumnName("tinyint2");
    modelBuilder.Entity<TestValueTypeEntity>().Property(t => t.Guid2).HasColumnName("guid2");
    modelBuilder.Entity<TestValueTypeEntity>().Property(t => t.VarBinary2).HasColumnName("varbinary2");
    modelBuilder.Entity<TestValueTypeEntity>().Property(t => t.VarChar2).HasColumnName("varchar2");
    
    modelBuilder.Entity<TestPKGuidEntity>().ToTable("test_pk_guid");
    modelBuilder.Entity<TestPKGuidEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<TestPKGuidEntity>().Property(t => t.Id).HasColumnName("test_pk_guid_id");
    modelBuilder.Entity<TestPKGuidEntity>().Property(t => t.Name).HasColumnName("name");
    
    modelBuilder.Entity<TestPKStringEntity>().ToTable("test_pk_string");
    modelBuilder.Entity<TestPKStringEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<TestPKStringEntity>().Property(t => t.Id).HasColumnName("test_pk_string_id");
    modelBuilder.Entity<TestPKStringEntity>().Property(t => t.Name).HasColumnName("name");
  }
}