﻿using ACore.AppTest.Modules.TestModule.Storages.SQL.Models;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Storages.EF;
using ACore.Server.Storages.Scripts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Exception = System.Exception;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ACore.AppTest.Modules.TestModule.Storages.SQL;

internal abstract class TestModuleSqlStorageImpl(DbContextOptions options, IMediator mediator, ILogger<TestModuleSqlStorageImpl> logger, IAuditConfiguration auditConfiguration)
  : AuditableDbContext(options, mediator, logger, auditConfiguration), ITestStorageModule
{
  protected abstract long IdLongGenerator<T>() where T : class;
  protected abstract int IdIntGenerator<T>() where T : class;
  protected abstract string IdStringGenerator<T>() where T : class;
  protected abstract Guid IdGuidGenerator<T>() where T : class;

  public override DbScriptBase UpdateScripts => new ScriptRegistrations();
  protected override string ModuleName => nameof(ITestStorageModule);

  internal DbSet<TestEntity> Tests { get; set; }
  internal DbSet<TestManualAuditEntity> TestManualAudits { get; set; }
  internal DbSet<TestAttributeAuditEntity> TestAttributeAudits { get; set; }
  internal DbSet<TestValueTypeEntity> TestValueTypes { get; set; }
  internal DbSet<TestPKGuidEntity> TestPKGuid { get; set; }
  internal DbSet<TestPKPKStringEntity> TestPKString { get; set; }

  public override async Task<TEntity?> Get<TEntity, TPK>(TPK id) where TEntity : class
  {
    if (id == null)
      throw new ArgumentNullException($"{nameof(id)} is null.");

    var res = typeof(TEntity) switch
    {
      { } entityType when entityType == typeof(TestEntity) => await Tests.FindAsync(Convert.ToInt32(id)) as TEntity,
      { } entityType when entityType == typeof(TestAttributeAuditEntity) => await TestAttributeAudits.FindAsync(Convert.ToInt32(id)) as TEntity,
      { } entityType when entityType == typeof(TestManualAuditEntity) => await TestManualAudits.FindAsync(Convert.ToInt64(id)) as TEntity,
      { } entityType when entityType == typeof(TestPKGuidEntity) => await TestPKGuid.FindAsync((Guid)Convert.ChangeType(id, typeof(Guid))) as TEntity,
      { } entityType when entityType == typeof(TestPKPKStringEntity) => await TestPKString.FindAsync(id.ToString()) as TEntity,
      { } entityType when entityType == typeof(TestValueTypeEntity) => await TestPKString.FindAsync(Convert.ToInt32(id)) as TEntity,
      _ => throw new Exception($"Unknown entity data type {typeof(TEntity).Name} with primary key {id}.")
    };
    return res ?? throw new ArgumentNullException(nameof(res), @"Save function returned null value.");
  }

  public async Task<TPK> Save<TEntity, TPK>(TEntity data) where TEntity : class
  {
    ArgumentNullException.ThrowIfNull(data);

    var res = typeof(TPK) switch
    {
      { } entityType when entityType == typeof(int) => (TPK)Convert.ChangeType(
        data switch
        {
          TestEntity testData
            => await SaveInternalWithAudit(testData, testData.Id,
              async (a) => await Tests.AddAsync(a),
              (i) => i.Id = IdIntGenerator<TestEntity>()),
          TestAttributeAuditEntity testAttributeAuditData
            => await SaveInternalWithAudit(testAttributeAuditData, testAttributeAuditData.Id,
              async (a) => await TestAttributeAudits.AddAsync(a),
              (i) => i.Id = IdIntGenerator<TestAttributeAuditEntity>()),
          TestValueTypeEntity testValueTypeData
            => await SaveInternalWithAudit(testValueTypeData, testValueTypeData.Id,
              async (a) => await TestValueTypes.AddAsync(a),
              (i) => i.Id = IdIntGenerator<TestValueTypeEntity>()),

          _ => throw new Exception($"Save is not allowed for {data.GetType().Name}")
        }, typeof(TPK)),

      { } entityType when entityType == typeof(long) => (TPK)Convert.ChangeType(data switch
      {
        TestManualAuditEntity testValueTypeData
          => await SaveInternalWithAudit(testValueTypeData, testValueTypeData.Id,
            async (a) => await TestManualAudits.AddAsync(a),
            (i) => i.Id = IdLongGenerator<TestManualAuditEntity>()),
        _ => throw new Exception($"Save is not allowed for {data.GetType().Name}")
      }, typeof(TPK)),
      { } entityType when entityType == typeof(string) => (TPK)Convert.ChangeType(data switch
      {
        TestPKPKStringEntity testAttributeAuditData
          => await SaveInternalWithAudit(testAttributeAuditData, testAttributeAuditData.Id,
            async (a) => await TestPKString.AddAsync(a),
            (i) => i.Id = IdStringGenerator<TestPKPKStringEntity>()),

        _ => throw new Exception($"Save is not allowed for {data.GetType().Name}")
      }, typeof(TPK)),

      { } entityType when entityType == typeof(Guid) => (TPK)Convert.ChangeType(data switch
      {
        TestPKGuidEntity testAttributeAuditData
          => await SaveInternalWithAudit(testAttributeAuditData, testAttributeAuditData.Id,
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
      case { } entityType when entityType == typeof(TestEntity):
        await DeleteInternalWithAudit<TestEntity, TPK>(id,
          (i) => Tests.Remove(i));
        return;
      case { } entityType when entityType == typeof(TestAttributeAuditEntity):
        await DeleteInternalWithAudit<TestAttributeAuditEntity, TPK>(id,
          (i) => TestAttributeAudits.Remove(i));
        return;
      case { } entityType when entityType == typeof(TestManualAuditEntity):
        await DeleteInternalWithAudit<TestManualAuditEntity, TPK>(id,
          (i) => TestManualAudits.Remove(i));
        return;
      default:
        throw new Exception($"Delete is not allowed for {typeof(T).Name}");
    }
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