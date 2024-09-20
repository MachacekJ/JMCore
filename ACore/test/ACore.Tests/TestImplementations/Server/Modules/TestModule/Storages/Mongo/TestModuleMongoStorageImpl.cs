using ACore.Server.Storages.EF;
using ACore.Server.Storages.Models;
using ACore.Server.Storages.Scripts;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.Mongo.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.EntityFrameworkCore.Extensions;
using ScriptRegistrations = ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.Mongo.Scripts.ScriptRegistrations;

// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.Mongo;

internal class TestModuleMongoStorageImpl : AuditableDbContext, ITestStorageModule
{
  public override DbScriptBase UpdateScripts => new ScriptRegistrations();
  protected override string ModuleName => nameof(ITestStorageModule);
  public override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Mongo);

  public TestModuleMongoStorageImpl(DbContextOptions<TestModuleMongoStorageImpl> options, IMediator mediator, ILogger<TestModuleMongoStorageImpl> logger) : base(options, mediator, logger)
  {
    RegisterDbSet(TestAttributeAudits);
  }

  internal DbSet<TestAttributeAuditPKMongoEntity> TestAttributeAudits { get; set; }

  // public DbSet<TestRootCategory> TestParents { get; set; }
  //public DbSet<TestCategory> TestChildren { get; set; }

  public async Task Save<TEntity, TPK>(TEntity data, string? hashToCheck = null)
    where TEntity : class
    => await SaveWithAudit<TEntity, TPK>(data, hashToCheck);


  public async Task Delete<T, TPK>(TPK id)
    where T : class
    => await DeleteWithAudit<T, TPK>(id);

  public DbSet<TEntity> DbSet<TEntity>() where TEntity : class
  {
    var res = typeof(TEntity) switch
    {
      // { } entityType when entityType == typeof(TestEntity) => Tests as DbSet<TEntity>,
      { } entityType when entityType == typeof(TestAttributeAuditPKMongoEntity) => TestAttributeAudits as DbSet<TEntity>,
      // { } entityType when entityType == typeof(TestPKGuidEntity) => TestPKGuid as DbSet<TEntity>,
      // { } entityType when entityType == typeof(TestManualAuditEntity) => TestManualAudits as DbSet<TEntity>,
      // { } entityType when entityType == typeof(TestPKStringEntity) => TestPKString as DbSet<TEntity>,
      // { } entityType when entityType == typeof(TestValueTypeEntity) => TestValueTypes as DbSet<TEntity>,
      _ => throw new Exception($"Unknown entity type {typeof(TEntity).Name}.")
    };
    return res ?? throw new ArgumentNullException(nameof(res), @"DbSet function returned null value.");
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<TestAttributeAuditPKMongoEntity>().ToCollection(DefaultNames.ObjectNameMapping[nameof(TestAttributeAuditPKMongoEntity)].TableName);
    modelBuilder.Entity<TestAttributeAuditPKMongoEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<TestAttributeAuditPKMongoEntity>(builder =>
      builder.Property(entity => entity.Id).HasElementName("_id")
    );
  }
}