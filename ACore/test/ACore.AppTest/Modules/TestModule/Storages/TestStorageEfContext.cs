using ACore.AppTest.Modules.TestModule.Storages.Models;
using ACore.Server.Modules.AuditModule.EF;
using ACore.Server.Storages.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ACore.AppTest.Modules.TestModule.Storages;

internal abstract class TestStorageEfContext(DbContextOptions options, IMediator mediator, ILogger<TestStorageEfContext> logger, IAuditDbService auditService)
  : DbContextBase(options, mediator, logger, auditService), ITestStorageModule
{
  public override DbScriptBase UpdateScripts => new ScriptRegistrations();
  protected override string ModuleName => nameof(ITestStorageModule);

  public DbSet<TestEntity> Tests { get; set; } = null!;
  public DbSet<TestManualAuditEntity> TestManualAudits { get; set; }
  public DbSet<TestAttributeAuditEntity> TestAttributeAudits { get; set; }
  public DbSet<TestValueTypeEntity> TestValueTypes { get; set; }
  internal DbSet<TestPKGuidEntity> TestPKGuid { get; set; }
  public DbSet<TestPKStringEntity> TestPKString { get; set; }

  public async Task SaveAsync(TestPKGuidEntity item)
  {
    await TestPKGuid.AddAsync(item);
    await SaveChangesAsync();
  }

  public async Task SaveAsync(TestPKStringEntity item)
  {
    await TestPKString.AddAsync(item);
    await SaveChangesAsync();
  }

  public async Task SaveAsync(TestValueTypeEntity item)
  {
    await TestValueTypes.AddAsync(item);
    await SaveChangesAsync();
  }
  public async Task SaveAsync(TestEntity item)
  {
    await Tests.AddAsync(item);
    await SaveChangesAsync();
  }
  
  public async Task SaveAsync(TestAttributeAuditEntity item)
  {
    await TestAttributeAudits.AddAsync(item);
    await SaveChangesAsync();
  }

  public async Task SaveAsync(TestManualAuditEntity item)
  {
    await TestManualAudits.AddAsync(item);
    await SaveChangesAsync();
  }
  
  public async Task DeleteAsync(TestAttributeAuditEntity item)
  {
    TestAttributeAudits.Remove(item);
    await SaveChangesAsync();
  }

  public async Task DeleteAsync(TestManualAuditEntity item)
  {
    TestManualAudits.Remove(item);
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
  public async Task<IEnumerable<TestManualAuditEntity>> AllTestManual()
  {
    return await TestManualAudits.ToArrayAsync();
  }

  public async Task<IEnumerable<TestPKGuidEntity>> AllTestPKGuid()
  {
    return await TestPKGuid.ToArrayAsync();
  }

  public async Task<IEnumerable<TestPKStringEntity>> AllTestPKString()
  {
    return await TestPKString.ToArrayAsync();
  }

  public async Task<IEnumerable<TestValueTypeEntity>> AllTestValueTypeString()
  {
    return await TestValueTypes.ToArrayAsync();
  }
}