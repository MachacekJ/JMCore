﻿using JMCore.Server.Storages.Base.Audit.EF;
using JMCore.Server.Storages.Base.EF;
using JMCore.Tests.ServerT.StoragesT.Impl.TestStorageModule.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace JMCore.Tests.ServerT.StoragesT.Impl.TestStorageModule;

public abstract class TestStorageEfContext(DbContextOptions options, IMediator mediator, ILogger<TestStorageEfContext> logger, IAuditDbService auditService)
  : DbContextBase(options, mediator, logger, auditService), ITestStorageModule
{
  private readonly ScriptRegistrations _dbSqlScript = new();

  public override DbScriptBase UpdateScripts => _dbSqlScript;
  public override string ModuleName => nameof(ITestStorageModule);

  public DbSet<TestEntity> Tests { get; set; } = null!;
  public DbSet<TestManualAuditEntity> TestManualAudits { get; set; }
  public DbSet<TestAttributeAuditEntity> TestAttributeAudits { get; set; }
  public DbSet<TestValueTypeEntity> TestValueTypes { get; set; }
  public DbSet<TestPKGuidEntity> TestPKGuid { get; set; }
  public DbSet<TestPKStringEntity> TestPKString { get; set; }

  public async Task AddAsync(TestValueTypeEntity item)
  {
    await TestValueTypes.AddAsync(item);
    await SaveChangesAsync();
  }
  public async Task AddAsync(TestEntity item)
  {
    await Tests.AddAsync(item);
    await SaveChangesAsync();
  }
  
  public async Task AddAsync(TestAttributeAuditEntity item)
  {
    await TestAttributeAudits.AddAsync(item);
    await SaveChangesAsync();
  }

  public async Task<IEnumerable<TestEntity>> AllTest()
  {
    return await Tests.ToArrayAsync();
  }
  
  public async Task<IEnumerable<TestAttributeAuditEntity>> AllTestAttribute()
  {
    return await TestAttributeAudits.ToArrayAsync();
  }
}