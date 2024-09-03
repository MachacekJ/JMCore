using ACore.AppTest.Modules.TestModule.Storages.SQL.Models;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Storages.EF;
using ACore.Server.Storages.Scripts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ACore.AppTest.Modules.TestModule.Storages.SQL;

internal abstract class TestModuleSqlStorageImpl : AuditableDbContext, ITestStorageModule
{
  public override DbScriptBase UpdateScripts => new ScriptRegistrations();
  protected override string ModuleName => nameof(ITestStorageModule);

  internal DbSet<TestEntity> Tests { get; set; }
  internal DbSet<TestManualAuditEntity> TestManualAudits { get; set; }
  internal DbSet<TestAttributeAuditEntity> TestAttributeAudits { get; set; }
  internal DbSet<TestValueTypeEntity> TestValueTypes { get; set; }
  internal DbSet<TestPKGuidEntity> TestPKGuid { get; set; }
  internal DbSet<TestPKPKStringEntity> TestPKString { get; set; }
  
  protected TestModuleSqlStorageImpl(DbContextOptions options, IMediator mediator, ILogger<TestModuleSqlStorageImpl> logger, IAuditConfiguration auditConfiguration) : base(options, mediator, logger, auditConfiguration)
  {
    RegisterDbSet(Tests);
    RegisterDbSet(TestManualAudits);
    RegisterDbSet(TestAttributeAudits);
    RegisterDbSet(TestValueTypes);
    RegisterDbSet(TestPKGuid);
    RegisterDbSet(TestPKString);
  }
  
  public async Task Save<TEntity, TPK>(TEntity data) where TEntity : class
    => await SaveWithAudit<TEntity, TPK>(data);
  
  public async Task Delete<T, TPK>(TPK id) where T : class
    => await DeleteWithAudit<T, TPK>(id);

  public DbSet<TEntity> DbSet<TEntity>() where TEntity : class
    => GetDbSet<TEntity>();
}