﻿using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Storages.Models;
using ACore.Server.Storages.Scripts;
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

  public override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Postgres);
  public override DbScriptBase UpdateScripts => new ScriptRegistrations();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    SetDatabaseNames<TestEntity>(modelBuilder);
    SetDatabaseNames<TestAttributeAuditPKIntEntity>(modelBuilder);
  //  SetDatabaseNames<TestManualAuditEntity>(modelBuilder);
    SetDatabaseNames<TestValueTypeEntity>(modelBuilder);
    SetDatabaseNames<TestPKGuidEntity>(modelBuilder);
    SetDatabaseNames<TestPKStringEntity>(modelBuilder);
  }
  
  private static void SetDatabaseNames<T>(ModelBuilder modelBuilder) where T : class => SetDatabaseNames<T>(DefaultNames.ObjectNameMapping, modelBuilder);
}