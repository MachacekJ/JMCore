using JMCore.Server.Modules.AuditModule.EF;
using JMCore.Server.Storages.EF;
using JMCore.Server.Storages.Models;
using JMCore.Tests.Implementations.Modules.TestModule.Storages.Models;
using JMCore.Tests.Implementations.Modules.TestModule.Storages.PG.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
//using JMCore.Server.PGStorage;
using ScriptRegistrations = JMCore.Tests.Implementations.Modules.TestModule.Storages.PG.Scripts.ScriptRegistrations;

namespace JMCore.Tests.Implementations.Modules.TestModule.Storages.PG;

public class TestPGEfStorageImpl(DbContextOptions<TestPGEfStorageImpl> options, IMediator mediator, ILogger<TestPGEfStorageImpl> logger, IAuditDbService auditService)
  : TestStorageEfContext(options, mediator, logger, auditService)
{
  public DbSet<TestMenuEntity> TestMenus { get; set; }
  public DbSet<TestCategoryEntity> TestCategories { get; set; }

  public override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Postgres);
  public override DbScriptBase UpdateScripts => new ScriptRegistrations();
  
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<TestEntity>().Ignore(a => a.UId);
    modelBuilder.Entity<TestAttributeAuditEntity>().Ignore(a => a.UId);
    
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<TestEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<TestAttributeAuditEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<TestManualAuditEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<TestValueTypeEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<TestPKGuidEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<TestPKStringEntity>().HasKey(p => p.Id);
    
    SetDatabaseNames<TestEntity>(modelBuilder);
    SetDatabaseNames<TestAttributeAuditEntity>(modelBuilder);
    SetDatabaseNames<TestManualAuditEntity>(modelBuilder);
    SetDatabaseNames<TestValueTypeEntity>(modelBuilder);
    SetDatabaseNames<TestPKGuidEntity>(modelBuilder);
    SetDatabaseNames<TestPKStringEntity>(modelBuilder);
  }
  private static void SetDatabaseNames<T>(ModelBuilder modelBuilder) where T : class => SetDatabaseNames<T>(TestPGEfDbNames.ObjectNameMapping, modelBuilder);
}