using ACore.AppTest.Modules.TestModule.Storages.EF;
using ACore.AppTest.Modules.TestModule.Storages.Mongo.Models;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.AuditModule.EF;
using ACore.Server.Storages.EF;
using ACore.Server.Storages.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore.Extensions;

// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace ACore.AppTest.Modules.TestModule.Storages.Mongo;

internal class EfTestMongoStorageImpl(DbContextOptions<EfTestMongoStorageImpl> options, IMediator mediator, ILogger<EfTestMongoStorageImpl> logger, IAuditDbService auditService, IAuditConfiguration auditConfiguration)
  : AuditableDbContext(options, mediator, logger, auditService, auditConfiguration), IEFTestStorageModule
{
  public override DbScriptBase UpdateScripts => new Scripts.ScriptRegistrations();
  protected override string ModuleName => nameof(IEFTestStorageModule);

  internal DbSet<TestAttributeAuditMongoEntity> TestAttributeAudits { get; set; }

  public override async Task<TEntity?> Get<TEntity, TPK>(TPK id) where TEntity : class
  {
    if (id == null)
      throw new ArgumentNullException($"{nameof(id)} is null.");

    var res = typeof(TEntity) switch
    {
      // { } entityType when entityType == typeof(TestEntity) => await Tests.FindAsync(Convert.ToInt32(id)) as TEntity,
      { } entityType when entityType == typeof(TestAttributeAuditMongoEntity) => await TestAttributeAudits.FindAsync(Convert.ToInt32(id)) as TEntity,
      //{ } entityType when entityType == typeof(TestManualAuditEntity) => await TestManualAudits.FindAsync(Convert.ToInt64(id)) as TEntity,
      // { } entityType when entityType == typeof(TestPKGuidEntity) => await TestPKGuid.FindAsync((Guid)Convert.ChangeType(id, typeof(Guid))) as TEntity,
      // { } entityType when entityType == typeof(TestPKStringEntity) => await TestPKString.FindAsync(id.ToString()) as TEntity,
      // { } entityType when entityType == typeof(TestValueTypeEntity) => await TestPKString.FindAsync(Convert.ToInt32(id)) as TEntity,
      _ => throw new Exception($"Unknown entity data type {typeof(TEntity).Name} with primary key {id}.")
    };
    return res ?? throw new ArgumentNullException(nameof(res), @"Save function returned null value.");
  }


  private ObjectId IdBsomIdGenerator<T>() where T : class
  {
    return ObjectId.GenerateNewId();
  }
  
  // public DbSet<TestRootCategory> TestParents { get; set; }
  //public DbSet<TestCategory> TestChildren { get; set; }

  public override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Mongo);

  public async Task<TPK> Save<TEntity, TPK>(TEntity data) where TEntity : class
  {
    ArgumentNullException.ThrowIfNull(data);

    var res = (typeof(TPK)) switch
    {
      { } entityType when entityType == typeof(ObjectId) => (TPK)Convert.ChangeType(
        data switch
        {
          // TestEntity testData
          //   => await SaveInternalWithAudit(testData, testData.Id,
          //     async (a) => await Tests.AddAsync(a),
          //     (i) => i.Id = IdIntGenerator<TestEntity>()),
          TestAttributeAuditMongoEntity testAttributeAuditData
            => await SaveInternalWithAudit(testAttributeAuditData, testAttributeAuditData.Id,
              async (a) => await TestAttributeAudits.AddAsync(a),
              (i) => i.Id = IdBsomIdGenerator<TestAttributeAuditMongoEntity>()),
          // TestValueTypeEntity testValueTypeData
          //   => await SaveInternalWithAudit(testValueTypeData, testValueTypeData.Id,
          //     async (a) => await TestValueTypes.AddAsync(a),
          //     (i) => i.Id = IdIntGenerator<TestValueTypeEntity>()),

          _ => throw new Exception($"Save is not allowed for {data.GetType().Name}")
        }, typeof(TPK)),

      // { } entityType when entityType == typeof(long) => (TPK)Convert.ChangeType(data switch
      // {
      //   TestManualAuditEntity testValueTypeData
      //     => await SaveInternalWithAudit(testValueTypeData, testValueTypeData.Id,
      //       async (a) => await TestManualAudits.AddAsync(a),
      //       (i) => i.Id = IdLongGenerator<TestManualAuditEntity>()),
      //   _ => throw new Exception($"Save is not allowed for {data.GetType().Name}")
      // }, typeof(TPK)),
      // { } entityType when entityType == typeof(string) => (TPK)Convert.ChangeType(data switch
      // {
      //   TestPKStringEntity testAttributeAuditData
      //     => await SaveInternalWithAudit(testAttributeAuditData, testAttributeAuditData.Id,
      //       async (a) => await TestPKString.AddAsync(a),
      //       (i) => i.Id = IdStringGenerator<TestPKStringEntity>()),
      //
      //   _ => throw new Exception($"Save is not allowed for {data.GetType().Name}")
      // }, typeof(TPK)),
      //
      // { } entityType when entityType == typeof(Guid) => (TPK)Convert.ChangeType(data switch
      // {
      //   TestPKGuidEntity testAttributeAuditData
      //     => await SaveInternalWithAudit(testAttributeAuditData, testAttributeAuditData.Id,
      //       async (a) => await TestPKGuid.AddAsync(a),
      //       (i) => i.Id = IdGuidGenerator<TestPKGuidEntity>()),
      //
      //   _ => throw new Exception($"Save is not allowed for {data.GetType().Name}")
      // }, typeof(TPK)),
      _ => throw new Exception($"Save is not allowed for primary key type {data.GetType().Name}")
    };
    return res ?? throw new ArgumentNullException(nameof(res), @"Save function returned null value.");
  }

  public async Task Delete<T, TPK>(TPK id) where T : class
  {
    switch (typeof(T))
    {
      // case { } entityType when entityType == typeof(TestEntity):
      //   await DeleteInternalWithAudit<TestEntity, TPK>(id,
      //     (i) => Tests.Remove(i));
      //   return;
      case { } entityType when entityType == typeof(TestAttributeAuditMongoEntity):
        await DeleteInternalWithAudit<TestAttributeAuditMongoEntity, TPK>(id,
          (i) => TestAttributeAudits.Remove(i));
        return;
      // case { } entityType when entityType == typeof(TestManualAuditEntity):
      //   await DeleteInternalWithAudit<TestManualAuditEntity, TPK>(id,
      //     (i) => TestManualAudits.Remove(i));
      //   return;
      default:
        throw new Exception($"Delete is not allowed for {typeof(T).Name}");
    }
  }

  public DbSet<TEntity> DbSet<TEntity>() where TEntity : class
  {
    var res = typeof(TEntity) switch
    {
      // { } entityType when entityType == typeof(TestEntity) => Tests as DbSet<TEntity>,
      { } entityType when entityType == typeof(TestAttributeAuditMongoEntity) => TestAttributeAudits as DbSet<TEntity>,
      // { } entityType when entityType == typeof(TestPKGuidEntity) => TestPKGuid as DbSet<TEntity>,
      // { } entityType when entityType == typeof(TestManualAuditEntity) => TestManualAudits as DbSet<TEntity>,
      // { } entityType when entityType == typeof(TestPKStringEntity) => TestPKString as DbSet<TEntity>,
      // { } entityType when entityType == typeof(TestValueTypeEntity) => TestValueTypes as DbSet<TEntity>,
      _ => throw new Exception($"Unknown entity type {typeof(TEntity).Name}.")
    };
    return res ?? throw new ArgumentNullException(nameof(res), @"DbSet function returned null value.");
  }

  protected override bool IsNew<TU>(TU id)
  {
    if (id == null)
      ArgumentNullException.ThrowIfNull(id);
    return typeof(TU) switch
    {
      { } entityType when entityType == typeof(ObjectId) => (ObjectId)Convert.ChangeType(id, typeof(ObjectId)) == ObjectId.Empty,
      _ => throw new Exception("Unknown primary data type {}")
    };
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    //  modelBuilder.Entity<TestEntity>().ToCollection(TestCollectionName);
    modelBuilder.Entity<TestAttributeAuditMongoEntity>().ToCollection(MongoTestStorageDbNames.ObjectNameMapping[nameof(TestAttributeAuditMongoEntity)].TableName);
    //  modelBuilder.Entity<TestRootCategory>().ToCollection(TestRootCategoryCollectionName);
    //  modelBuilder.Entity<TestCategory>().ToCollection(TestCategoryCollectionName);

    // modelBuilder.Entity<TestEntity>().HasKey(p => p.UId);
    modelBuilder.Entity<TestAttributeAuditMongoEntity>().HasKey(p => p.Id);
    //
    modelBuilder.Entity<TestAttributeAuditMongoEntity>(builder =>
      builder.Property(entity => entity.Id).HasElementName("_id")
    );
    // modelBuilder.Entity<TestAttributeAuditEntity>(builder =>
    //   builder.Property(entity => entity.UId).HasElementName("_id")
    // );
  }
}