using ACore.AppTest.Modules.TestModule.Storages.SQL.Models;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Storages.Models;
using ACore.Server.Storages.Scripts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.AppTest.Modules.TestModule.Storages.SQL.PG;

using ScriptRegistrations = Scripts.ScriptRegistrations;

internal class TestModulePGStorageImpl(DbContextOptions<TestModulePGStorageImpl> options, IMediator mediator, ILogger<TestModulePGStorageImpl> logger, IAuditConfiguration auditConfiguration)
  : TestModuleSqlStorageImpl(options, mediator, logger, auditConfiguration)
{
  public DbSet<TestMenuEntity> TestMenus { get; set; }
  public DbSet<TestCategoryEntity> TestCategories { get; set; }

  public override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Postgres);
  public override DbScriptBase UpdateScripts => new ScriptRegistrations();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
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

  protected override int IdIntGenerator<T>()
  {
    return 0;
  }

  protected override long IdLongGenerator<T>()
  {
    return 0;
  }

  protected override string IdStringGenerator<T>()
  {
    return IdGuidGenerator<T>().ToString();
  }

  protected override Guid IdGuidGenerator<T>()
  {
    return Guid.NewGuid();
  }

  private static void SetDatabaseNames<T>(ModelBuilder modelBuilder) where T : class => SetDatabaseNames<T>(DefaultNames.ObjectNameMapping, modelBuilder);
}