using ACore.AppTest.Modules.TestModule.Models;
using ACore.AppTest.Modules.TestModule.Storages.EF.Models;
using ACore.Server.Modules.AuditModule.EF;
using ACore.Server.Storages.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ACore.AppTest.Modules.TestModule.Storages.EF;

internal abstract class EFTestStorageContext(DbContextOptions options, IMediator mediator, ILogger<EFTestStorageContext> logger, IAuditDbService auditService)
  : DbContextBase(options, mediator, logger, auditService), IEFTestStorageModule
{
  protected abstract int IdIntGenerator<T>() where T : class;
  protected abstract string IdStringGenerator<T>() where T : class;
  protected abstract Guid IdGuidGenerator<T>() where T : class;

  public override DbScriptBase UpdateScripts => new ScriptRegistrations();
  protected override string ModuleName => nameof(IEFTestStorageModule);

  public DbSet<TestEntity> Tests { get; set; } = null!;
  public DbSet<TestManualAuditEntity> TestManualAudits { get; set; }
  public DbSet<TestAttributeAuditEntity> TestAttributeAudits { get; set; }
  public DbSet<TestValueTypeEntity> TestValueTypes { get; set; }
  internal DbSet<TestPKGuidEntity> TestPKGuid { get; set; }
  public DbSet<TestPKStringEntity> TestPKString { get; set; }

  public override async Task<T?> Get<T, TU>(TU id) where T : class
  {
    if (id == null)
      throw new ArgumentNullException($"{nameof(id)} is null.");

    var res = typeof(T) switch
    {
      { } entityType when entityType == typeof(TestEntity) => await Tests.FindAsync(Convert.ToInt32(id)) as T,
      { } entityType when entityType == typeof(TestAttributeAuditEntity) => await TestAttributeAudits.FindAsync(Convert.ToInt32(id)) as T,
      { } entityType when entityType == typeof(TestPKGuidEntity) => await TestPKGuid.FindAsync((Guid)Convert.ChangeType(id, typeof(Guid))) as T,
      { } entityType when entityType == typeof(TestManualAuditData) => await TestManualAudits.FindAsync(Convert.ToInt64(id)) as T,
      { } entityType when entityType == typeof(TestPKStringEntity) => await TestPKString.FindAsync(id.ToString()) as T,
      { } entityType when entityType == typeof(TestValueTypeEntity) => await TestPKString.FindAsync(Convert.ToInt32(id)) as T,
      _ => throw new Exception($"Unknown entity data type {typeof(T).Name} with primary key {id}.")
    };
    return res ?? throw new ArgumentNullException(nameof(res), @"Save function returned null value.");
  }

  public async Task<TPK> Save<TEntity, TPK>(TEntity data) where TEntity : class
  {
    ArgumentNullException.ThrowIfNull(data);

    var res = (typeof(TPK)) switch
    {
      { } entityType when entityType == typeof(int) => (TPK)Convert.ChangeType(
        data switch
        {
          TestEntity testData
            => await SaveInternal<TestEntity, int>(data, testData.Id,
              async (a) => await Tests.AddAsync(a),
              (i) => i.Id = IdIntGenerator<TestEntity>()),
          TestAttributeAuditEntity testAttributeAuditData
            => await SaveInternal<TestAttributeAuditEntity, int>(data, testAttributeAuditData.Id,
              async (a) => await TestAttributeAudits.AddAsync(a),
              (i) => i.Id = IdIntGenerator<TestAttributeAuditEntity>()),
          TestValueTypeEntity testValueTypeData
            => await SaveInternal<TestValueTypeEntity, int>(data, testValueTypeData.Id,
              async (a) => await TestValueTypes.AddAsync(a),
              (i) => i.Id = IdIntGenerator<TestValueTypeEntity>()),

          _ => throw new Exception($"Save is not allowed for {data.GetType().Name}")
        }, typeof(TPK)),
      
      { } entityType when entityType == typeof(long) => (TPK)Convert.ChangeType(data switch
      {
        TestManualAuditEntity testValueTypeData
          => await SaveInternal<TestManualAuditEntity, long>(data, testValueTypeData.Id,
            async (a) => await TestManualAudits.AddAsync(a),
            (i) => i.Id = IdIntGenerator<TestManualAuditEntity>()),
        _ => throw new Exception($"Save is not allowed for {data.GetType().Name}")
      }, typeof(TPK)),
      { } entityType when entityType == typeof(string) => (TPK)Convert.ChangeType(data switch
      {
        TestPKStringEntity testAttributeAuditData
          => await SaveInternal<TestPKStringEntity, string>(data, testAttributeAuditData.Id,
            async (a) => await TestPKString.AddAsync(a),
            (i) => i.Id = IdStringGenerator<TestPKStringEntity>()),

        _ => throw new Exception($"Save is not allowed for {data.GetType().Name}")
      }, typeof(TPK)),
      
      { } entityType when entityType == typeof(Guid) => (TPK)Convert.ChangeType(data switch
      {
        TestPKGuidEntity testAttributeAuditData
          => await SaveInternal<TestPKGuidEntity, Guid>(data, testAttributeAuditData.Id,
            async (a) => await TestPKGuid.AddAsync(a),
            (i) => i.Id = IdGuidGenerator<TestPKGuidEntity>()),

        _ => throw new Exception($"Save is not allowed for {data.GetType().Name}")
      }, typeof(TPK)),
      _ => throw new Exception($"Save is not allowed for primary key type {data.GetType().Name}")
    };
    return res ?? throw new ArgumentNullException(nameof(res), @"Save function returned null value.");
  }

  public async Task Delete<T, TPK>(TPK id) where T : class
  {
    switch (typeof(T))
    {
      case { } entityType when entityType == typeof(TestData):
        await DeleteInternal<TestEntity, TPK>(id,
          (i) => Tests.Remove(i));
        return;
      case { } entityType when entityType == typeof(TestAttributeAuditData):
        await DeleteInternal<TestAttributeAuditEntity, TPK>(id,
          (i) => TestAttributeAudits.Remove(i));
        return;
      case { } entityType when entityType == typeof(TestManualAuditData):
        await DeleteInternal<TestManualAuditEntity, TPK>(id,
          (i) => TestManualAudits.Remove(i));
        return;
      default:
        throw new Exception($"Delete is not allowed for {typeof(T).Name}");
    }
  }

  public async Task<T[]> All<T>() where T : class
  {
    return typeof(T) switch
    {
      { } entityType when entityType == typeof(TestEntity) => await Tests.ToArrayAsync() as T[] ?? [],
      { } entityType when entityType == typeof(TestAttributeAuditEntity) => await TestAttributeAudits.ToArrayAsync() as T[] ?? [],
      { } entityType when entityType == typeof(TestManualAuditEntity) => await TestManualAudits.ToArrayAsync() as T[] ?? [],
      { } entityType when entityType == typeof(TestPKGuidEntity) => await TestPKGuid.ToArrayAsync() as T[] ?? [],
      { } entityType when entityType == typeof(TestPKStringEntity) => await TestPKString.ToArrayAsync() as T[] ?? [],
      { } entityType when entityType == typeof(TestValueTypeEntity) => await TestValueTypes.ToArrayAsync() as T[] ?? [],
      _ => throw new Exception($"Entity '{typeof(T).Name}' is not registered.")
    };
  }
}