using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Storages.Base.Audit.EF;
using JMCore.Server.Storages.Base.EF;
using JMCore.Tests.ServerT.StoragesT.Implementations.TestStorageModule;
using JMCore.Tests.ServerT.StoragesT.Implementations.TestStorageModule.Models;
using JMCore.TestsIntegrations.ServerT.StoragesT.TestStorageImplementations.Mongo.TestStorageModule.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.EntityFrameworkCore.Extensions;

namespace JMCore.TestsIntegrations.ServerT.StoragesT.TestStorageImplementations.Mongo.TestStorageModule;

public class TestMongoStorageImpl(DbContextOptions<TestMongoStorageImpl> options, IMediator mediator, ILogger<TestMongoStorageImpl> logger, IAuditDbService auditService) : TestStorageEfContext(options, mediator, logger, auditService)
{
  public const string TestCollectionName = "test";
  public const string TestAttributeCollectionName = "testAtrribute";
  public const string TestRootCategoryCollectionName = "testRootCategory";
  public const string TestCategoryCollectionName = "testCategory";
  public DbSet<TestRootCategory> TestParents { get; set; }
  //public DbSet<TestCategory> TestChildren { get; set; }
  
  protected override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Mongo);
  public override DbScriptBase UpdateScripts => new Scripts.ScriptRegistrations();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<TestEntity>().Ignore(t => t.Id);
    modelBuilder.Entity<TestAttributeAuditEntity>().Ignore(t => t.Id);

    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<TestEntity>().ToCollection(TestCollectionName);
    modelBuilder.Entity<TestAttributeAuditEntity>().ToCollection(TestAttributeCollectionName);
    modelBuilder.Entity<TestRootCategory>().ToCollection(TestRootCategoryCollectionName);
    modelBuilder.Entity<TestCategory>().ToCollection(TestCategoryCollectionName);
    
    modelBuilder.Entity<TestEntity>().HasKey(p => p.UId);
    modelBuilder.Entity<TestAttributeAuditEntity>().HasKey(p => p.UId);
    
    modelBuilder.Entity<TestEntity>(builder =>
      builder.Property(entity => entity.UId).HasElementName("_id")
    );
    modelBuilder.Entity<TestAttributeAuditEntity>(builder =>
      builder.Property(entity => entity.UId).HasElementName("_id")
    );

  }
}