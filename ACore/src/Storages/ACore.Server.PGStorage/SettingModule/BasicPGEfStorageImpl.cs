using ACore.Server.Modules.AuditModule.EF;
using ACore.Server.Modules.SettingModule.Storage.BaseImpl;
using ACore.Server.Modules.SettingModule.Storage.Models;
using ACore.Server.Storages.EF;
using ACore.Server.Storages.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScriptRegistrations = ACore.Server.PGStorage.SettingModule.Scripts.ScriptRegistrations;

namespace ACore.Server.PGStorage.SettingModule;

using ScriptRegistrations = Scripts.ScriptRegistrations;

public class BasicSqlPGEfStorageImpl : BasicSqlStorageImpl
{
  public override DbScriptBase UpdateScripts => new ScriptRegistrations();
  public override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Postgres);

  public BasicSqlPGEfStorageImpl(DbContextOptions<BasicSqlPGEfStorageImpl> options, IMediator mediator, ILogger<BasicSqlPGEfStorageImpl> logger) : base(options, mediator, logger)
  {
  }

  public BasicSqlPGEfStorageImpl(DbContextOptions<BasicSqlPGEfStorageImpl> options, IMediator mediator, IAuditDbService? auditService, ILogger<BasicSqlPGEfStorageImpl> logger) : base(options, mediator, auditService, logger)
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