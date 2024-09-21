using ACore.Server.Storages.EF;
using ACore.Server.Storages.Scripts;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL;

internal abstract class TestModuleSqlStorageImpl : AuditableDbContext, ITestStorageModule
{
  public override DbScriptBase UpdateScripts => new ScriptRegistrations();
  protected override string ModuleName => nameof(ITestStorageModule);

  internal DbSet<TestNoAuditEntity> Tests { get; set; }
  internal DbSet<TestAuditEntity> TestAudits { get; set; }
  internal DbSet<TestValueTypeEntity> TestValueTypes { get; set; }
  internal DbSet<TestPKGuidEntity> TestPKGuid { get; set; }
  internal DbSet<TestPKStringEntity> TestPKString { get; set; }
  internal DbSet<TestPKLongEntity> TestPKLong { get; set; }
  
  protected TestModuleSqlStorageImpl(DbContextOptions options, IMediator mediator, ILogger<TestModuleSqlStorageImpl> logger) : base(options, mediator, logger)
  {
    RegisterDbSet(Tests);
    RegisterDbSet(TestAudits);
    RegisterDbSet(TestValueTypes);
    RegisterDbSet(TestPKGuid);
    RegisterDbSet(TestPKString);
    RegisterDbSet(TestPKLong);
  }
  
  public async Task Save<TEntity, TPK>(TEntity data, string? hashToCheck = null) where TEntity : class
    => await SaveWithAudit<TEntity, TPK>(data, hashToCheck);
  
  public async Task Delete<T, TPK>(TPK id) where T : class
    => await DeleteWithAudit<T, TPK>(id);

  public DbSet<TEntity> DbSet<TEntity>() where TEntity : class
    => GetDbSet<TEntity>();
}