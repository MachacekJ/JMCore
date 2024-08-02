using JMCore.Server.Modules.AuditModule.EF;
using JMCore.Server.Storages.EF;
using JMCore.Server.Storages.Models;
using JMCore.Tests.Implementations.Modules.TestModule.Storages.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace JMCore.Tests.Implementations.Modules.TestModule.Storages;

public abstract class TestStorageEfContext(DbContextOptions options, IMediator mediator, ILogger<TestStorageEfContext> logger, IAuditDbService auditService)
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

  public async Task AddAsync(TestPKGuidEntity item)
  {
    await TestPKGuid.AddAsync(item);
    await SaveChangesAsync();
  }

  public async Task AddAsync(TestPKStringEntity item)
  {
    await TestPKString.AddAsync(item);
    await SaveChangesAsync();
  }

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

  public async Task AddAsync(TestManualAuditEntity item)
  {
    await TestManualAudits.AddAsync(item);
    await SaveChangesAsync();
  }
  
  public async Task UpdateAsync(TestAttributeAuditEntity item)
  {
    await SaveChangesAsync();
  }
  
  public async Task UpdateAsync(TestManualAuditEntity item)
  {
    await SaveChangesAsync();
  }

  public async Task UpdateAsync(TestPKGuidEntity item)
  {
    await SaveChangesAsync();
  }

  public async Task UpdateAsync(TestPKStringEntity item)
  {
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
}