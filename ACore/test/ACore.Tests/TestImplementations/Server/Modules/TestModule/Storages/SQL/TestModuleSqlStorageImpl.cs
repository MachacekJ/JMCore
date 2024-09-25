using ACore.Server.Storages.EF;
using ACore.Server.Storages.Models.PK;
using ACore.Server.Storages.Scripts;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL;

internal abstract class TestModuleSqlStorageImpl : DbContextBase, ITestStorageModule
{
  protected override DbScriptBase UpdateScripts => new ScriptRegistrations();
  protected override string ModuleName => nameof(ITestStorageModule);

  internal DbSet<TestNoAuditEntity> TestNoAudits { get; set; }
  internal DbSet<TestAuditEntity> TestAudits { get; set; }
  internal DbSet<TestValueTypeEntity> TestValueTypes { get; set; }
  internal DbSet<TestPKGuidEntity> TestPKGuid { get; set; }
  internal DbSet<TestPKStringEntity> TestPKString { get; set; }
  internal DbSet<TestPKLongEntity> TestPKLong { get; set; }

  protected TestModuleSqlStorageImpl(DbContextOptions options, IMediator mediator, ILogger<TestModuleSqlStorageImpl> logger) : base(options, mediator, logger)
  {
    RegisterDbSet(TestNoAudits);
    RegisterDbSet(TestAudits);
    RegisterDbSet(TestValueTypes);
    RegisterDbSet(TestPKGuid);
    RegisterDbSet(TestPKString);
    RegisterDbSet(TestPKLong);
  }

  // public async Task Save<TEntity, TPK>(TEntity data, string? hashToCheck = null) where TEntity : class
  //   => await base.Save<TEntity, TPK>(data, hashToCheck);
  //
  // public async Task Delete<T, TPK>(TPK id) where T : class
  //   => await  base.Delete<T, TPK>(id);


  public async Task SaveTestEntity<TEntity, TPK>(TEntity data, string? hashToCheck = null)
    where TEntity : PKEntity<TPK>
    => await Save<TEntity, TPK>(data, hashToCheck);

  public async Task DeleteTestEntity<TEntity, TPK>(TPK id)
    where TEntity : PKEntity<TPK>
    => await Delete<TEntity, TPK>(id);

  public DbSet<TEntity> DbSet<TEntity, TPK>()
    where TEntity : PKEntity<TPK>
    => GetDbSet<TEntity>();
}