using ACore.AppTest.Modules.TestModule.Storages.SQL.Models;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Storages.EF;
using ACore.Server.Storages.Scripts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Exception = System.Exception;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ACore.AppTest.Modules.TestModule.Storages.SQL;

internal abstract class TestModuleSqlStorageImpl : AuditableDbContext, ITestStorageModule
{
  protected TestModuleSqlStorageImpl(DbContextOptions options, IMediator mediator, ILogger<TestModuleSqlStorageImpl> logger, IAuditConfiguration auditConfiguration) : base(options, mediator, logger, auditConfiguration)
  {
    RegisterDbSet(Tests);
    RegisterDbSet(TestManualAudits);
    RegisterDbSet(TestAttributeAudits);
    RegisterDbSet(TestValueTypes);
    RegisterDbSet(TestPKGuid);
    RegisterDbSet(TestPKString);
  }

  public override DbScriptBase UpdateScripts => new ScriptRegistrations();
  protected override string ModuleName => nameof(ITestStorageModule);

  internal DbSet<TestEntity> Tests { get; set; }
  internal DbSet<TestManualAuditEntity> TestManualAudits { get; set; }
  internal DbSet<TestAttributeAuditEntity> TestAttributeAudits { get; set; }
  internal DbSet<TestValueTypeEntity> TestValueTypes { get; set; }
  internal DbSet<TestPKGuidEntity> TestPKGuid { get; set; }
  internal DbSet<TestPKPKStringEntity> TestPKString { get; set; }
  
  public async Task<TPK> Save<TEntity, TPK>(TEntity data)
    where TEntity : class
    
  {
    ArgumentNullException.ThrowIfNull(data);
    return await SaveInternalWithAudit<TEntity, TPK>(data);

  }

  public async Task Delete<T, TPK>(TPK id)
    where T : class
    
  {
    await DeleteInternalWithAudit<T, TPK>(id);
  }

  public DbSet<TEntity> DbSet<TEntity>() where TEntity : class
  {
    var res = typeof(TEntity) switch
    {
      { } entityType when entityType == typeof(TestEntity) => Tests as DbSet<TEntity>,
      { } entityType when entityType == typeof(TestAttributeAuditEntity) => TestAttributeAudits as DbSet<TEntity>,
      { } entityType when entityType == typeof(TestPKGuidEntity) => TestPKGuid as DbSet<TEntity>,
      { } entityType when entityType == typeof(TestManualAuditEntity) => TestManualAudits as DbSet<TEntity>,
      { } entityType when entityType == typeof(TestPKPKStringEntity) => TestPKString as DbSet<TEntity>,
      { } entityType when entityType == typeof(TestValueTypeEntity) => TestValueTypes as DbSet<TEntity>,
      _ => throw new Exception($"Unknown entity type {typeof(TEntity).Name}.")
    };
    return res ?? throw new ArgumentNullException(nameof(res), @"DbSet function returned null value.");
  }
}