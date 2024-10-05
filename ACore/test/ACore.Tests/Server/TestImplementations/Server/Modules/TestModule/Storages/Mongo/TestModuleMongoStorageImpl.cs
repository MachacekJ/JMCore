using ACore.Server.Storages.Definitions.EF;
using ACore.Server.Storages.Definitions.EF.Base;
using ACore.Server.Storages.Definitions.EF.Base.Scripts;
using ACore.Server.Storages.Definitions.EF.MongoStorage;
using ACore.Server.Storages.Definitions.Models.PK;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.Mongo.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.EntityFrameworkCore.Extensions;
using ScriptRegistrations = ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.Mongo.Scripts.ScriptRegistrations;

// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.Mongo;

internal class TestModuleMongoStorageImpl : DbContextBase, ITestStorageModule
{
  protected override DbScriptBase UpdateScripts => new ScriptRegistrations();
  protected override string ModuleName => nameof(ITestStorageModule);
  protected override EFStorageDefinition EFStorageDefinition => new MongoStorageDefinition();

  public TestModuleMongoStorageImpl(DbContextOptions<TestModuleMongoStorageImpl> options, IMediator mediator, ILogger<TestModuleMongoStorageImpl> logger) : base(options, mediator, logger)
  {
    RegisterDbSet(TestAudits);
  }

  internal DbSet<TestPKMongoEntity> TestAudits { get; set; }

  public async Task SaveTestEntity<TEntity, TPK>(TEntity data, string? hashToCheck = null)
    where TEntity : PKEntity<TPK>
    => await Save<TEntity, TPK>(data, hashToCheck);
  
  
  public async Task DeleteTestEntity<TEntity, TPK>(TPK id)
    where TEntity : PKEntity<TPK>
    => await Delete<TEntity, TPK>(id);

  public DbSet<TEntity> DbSet<TEntity, TPK>()  where TEntity : PKEntity<TPK>
  {
    var res = typeof(TEntity) switch
    {
      { } entityType when entityType == typeof(TestPKMongoEntity) => TestAudits as DbSet<TEntity>,
      _ => throw new Exception($"Unknown entity type {typeof(TEntity).Name}.")
    };
    return res ?? throw new ArgumentNullException(nameof(res), @"DbSet function returned null value.");
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<TestPKMongoEntity>().ToCollection(DefaultNames.ObjectNameMapping[nameof(TestPKMongoEntity)].TableName);
    modelBuilder.Entity<TestPKMongoEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<TestPKMongoEntity>(builder =>
      builder.Property(entity => entity.Id).HasElementName("_id")
    );
  }
}