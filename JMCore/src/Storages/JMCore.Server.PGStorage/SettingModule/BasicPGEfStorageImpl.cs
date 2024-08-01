using JMCore.Server.Modules.AuditModule.EF;
using JMCore.Server.Modules.SettingModule.Storage.BaseImpl;
using JMCore.Server.Modules.SettingModule.Storage.Models;
using JMCore.Server.Storages.EF;
using JMCore.Server.Storages.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScriptRegistrations = JMCore.Server.PGStorage.SettingModule.Scripts.ScriptRegistrations;

namespace JMCore.Server.PGStorage.SettingModule;

public class BasicSqlPGEfStorageImpl : BasicSqlStorageImpl
{
  public override DbScriptBase UpdateScripts => new ScriptRegistrations();
  protected override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Postgres);

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