﻿using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.SettingModule.Storage.SQL.Models;
using ACore.Server.Storages.Models;
using ACore.Server.Storages.Scripts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Modules.SettingModule.Storage.SQL.Memory;

internal class SettingModuleSqlMemoryStorageImpl(DbContextOptions<SettingModuleSqlMemoryStorageImpl> options, IMediator mediator, IAuditConfiguration? auditConfiguration, ILogger<SettingModuleSqlMemoryStorageImpl> logger)
  : SettingModuleSqlStorageImpl(options, mediator, auditConfiguration, logger)
{
  public override DbScriptBase UpdateScripts => new ScriptRegistrations();
  public override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Memory);

  public SettingModuleSqlMemoryStorageImpl(DbContextOptions<SettingModuleSqlMemoryStorageImpl> options, IMediator mediator, ILogger<SettingModuleSqlMemoryStorageImpl> logger) : this(options, mediator, null, logger)
  {
  }

  protected override int IdIntGenerator<TEntity>()
  {
    return typeof(TEntity) switch
    {
      { } entityType when entityType == typeof(SettingEntity) => !Settings.Any() ? 1 : Settings.Max(i => i.Id) + 1,
      _ => throw new Exception($"Unknown entity type {typeof(TEntity).Name}.")
    };
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<SettingEntity>().HasKey(p => p.Key);
  }
}