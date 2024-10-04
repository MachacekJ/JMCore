using ACore.Server.Storages.Definitions.EF;
using ACore.Server.Storages.Definitions.EF.Base.Scripts;
using ACore.Server.Storages.Definitions.EF.PGStorage;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.PG;

using ScriptRegistrations = Scripts.ScriptRegistrations;

internal class TestModulePGStorageImpl(DbContextOptions<TestModulePGStorageImpl> options, IMediator mediator, ILogger<TestModulePGStorageImpl> logger)
  : TestModuleSqlStorageImpl(options, mediator, logger)
{
  public DbSet<TestMenuEntity> TestMenus { get; set; }
  public DbSet<TestCategoryEntity> TestCategories { get; set; }

  protected override EFStorageDefinition EFStorageDefinition => new PGStorageDefinition();
  protected override DbScriptBase UpdateScripts => new ScriptRegistrations();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    SetDatabaseNames<TestNoAuditEntity>(modelBuilder);
    SetDatabaseNames<TestAuditEntity>(modelBuilder);
  //  SetDatabaseNames<TestManualAuditEntity>(modelBuilder);
    SetDatabaseNames<TestValueTypeEntity>(modelBuilder);
    SetDatabaseNames<TestPKGuidEntity>(modelBuilder);
    SetDatabaseNames<TestPKStringEntity>(modelBuilder);
  }
  
  private static void SetDatabaseNames<T>(ModelBuilder modelBuilder) where T : class => SetDatabaseNames<T>(DefaultNames.ObjectNameMapping, modelBuilder);
}