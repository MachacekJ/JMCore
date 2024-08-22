using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.SettingModule.Storage.BaseImpl;
using ACore.Server.Modules.SettingModule.Storage.Models;
using ACore.Server.Storages.EF;
using ACore.Server.Storages.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Modules.SettingModule.Storage.PG;

using ScriptRegistrations = Scripts.ScriptRegistrations;

internal class BasicSqlPGEfStorageImpl : BasicSqlStorageImpl
{
  public override DbScriptBase UpdateScripts => new ScriptRegistrations();
  public override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Postgres);

  public BasicSqlPGEfStorageImpl(DbContextOptions<BasicSqlPGEfStorageImpl> options, IMediator mediator, ILogger<BasicSqlPGEfStorageImpl> logger) : base(options, mediator, logger)
  {
  }

  public BasicSqlPGEfStorageImpl(DbContextOptions<BasicSqlPGEfStorageImpl> options, IMediator mediator, IAuditConfiguration auditConfiguration , ILogger<BasicSqlPGEfStorageImpl> logger) : base(options, mediator, auditConfiguration, logger)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    //modelBuilder.Entity<SettingEntity>().Ignore(t => t.UId);
    
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<SettingEntity>().ToTable("setting");
    modelBuilder.Entity<SettingEntity>().HasKey(p => p.Id);
    modelBuilder.Entity<SettingEntity>().Property(t => t.Id).HasColumnName("setting_id");
    modelBuilder.Entity<SettingEntity>().Property(t => t.Key).HasColumnName("key");
    modelBuilder.Entity<SettingEntity>().Property(t => t.Value).HasColumnName("value");
    modelBuilder.Entity<SettingEntity>().Property(t => t.IsSystem).HasColumnName("is_system");

  }
}