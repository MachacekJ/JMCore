using ACore.AppTest.Modules.TestModule.Storages.EF;
using ACore.AppTest.Modules.TestModule.Storages.EF.Models;
using ACore.AppTest.Modules.TestModule.Storages.Mongo.Models;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.AuditModule.EF;
using ACore.Server.Storages.EF;
using ACore.Server.Storages.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.EntityFrameworkCore.Extensions;

namespace ACore.AppTest.Modules.TestModule.Storages.Mongo;

internal class EfTestMongoStorageImpl(DbContextOptions<EfTestMongoStorageImpl> options, IMediator mediator, ILogger<EfTestMongoStorageImpl> logger, IAuditDbService auditService, IAuditConfiguration auditConfiguration) 
  : EFTestStorageContext(options, mediator, logger, auditService, auditConfiguration)
{
  public const string TestCollectionName = "test";
  public const string TestAttributeCollectionName = "testAtrribute";
  public const string TestRootCategoryCollectionName = "testRootCategory";
  public const string TestCategoryCollectionName = "testCategory";
  public DbSet<TestRootCategory> TestParents { get; set; }
  //public DbSet<TestCategory> TestChildren { get; set; }
  
  public override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Mongo);
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
  protected override int IdIntGenerator<T>()
  {
    return 1;
  }

  protected override long IdLongGenerator<T>()
  {
    return 1;
  }
  protected override string IdStringGenerator<T>()
  {
    return IdGuidGenerator<T>().ToString();
  }

  protected override Guid IdGuidGenerator<T>()
  {
    return Guid.NewGuid();
  }
}