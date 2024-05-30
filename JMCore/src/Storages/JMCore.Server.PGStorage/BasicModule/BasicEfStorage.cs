﻿using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Storages.Base.Audit;
using JMCore.Server.Storages.Base.Audit.EF;
using JMCore.Server.Storages.Base.EF;
using JMCore.Server.Storages.Modules.BasicModule.EF;
using JMCore.Server.Storages.Modules.BasicModule.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScriptRegistrations = JMCore.Server.PGStorage.BasicModule.Scripts.ScriptRegistrations;

namespace JMCore.Server.PGStorage.BasicModule;

public class BasicEfStorage : BasicStorageEfContext
{
  public override DbScriptBase SqlScripts => new ScriptRegistrations();
  public override StorageTypeEnum StorageType => StorageTypeEnum.Postgres;

  public BasicEfStorage(DbContextOptions<BasicEfStorage> options, IMediator mediator, ILogger<BasicEfStorage> logger) : base(options, mediator, logger)
  {
  }

  public BasicEfStorage(DbContextOptions<BasicEfStorage> options, IMediator mediator, IAuditDbService? auditService, ILogger<BasicEfStorage> logger) : base(options, mediator, auditService, logger)
  {
  }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<SettingEntity>().ToTable("setting");
    modelBuilder.Entity<SettingEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<SettingEntity>().Property(t => t.Id).HasColumnName("setting_id");
    modelBuilder.Entity<SettingEntity>().Property(t => t.Key).HasColumnName("key");
    modelBuilder.Entity<SettingEntity>().Property(t => t.Value).HasColumnName("value");
    modelBuilder.Entity<SettingEntity>().Property(t => t.IsSystem).HasColumnName("is_system");
  }
}