using ACore.AppTest.Modules.TestModule.Storages.EF.Models;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.AuditModule.EF;
using ACore.Server.Storages.EF;
using ACore.Server.Storages.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.AppTest.Modules.TestModule.Storages.EF.PG;

using ScriptRegistrations = Scripts.ScriptRegistrations;

internal class PGEFTestStorageImpl(DbContextOptions<PGEFTestStorageImpl> options, IMediator mediator, ILogger<PGEFTestStorageImpl> logger, IAuditDbService auditService, IAuditConfiguration auditConfiguration) 
  : EFTestStorageContext(options, mediator, logger, auditService, auditConfiguration)
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

  private static void SetDatabaseNames<T>(ModelBuilder modelBuilder) where T : class => SetDatabaseNames<T>(PGTestStorageDbNames.ObjectNameMapping, modelBuilder);
}