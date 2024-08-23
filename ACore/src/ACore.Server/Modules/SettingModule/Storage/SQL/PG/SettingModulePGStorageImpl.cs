using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.SettingModule.Storage.SQL.Models;
using ACore.Server.Storages.Models;
using ACore.Server.Storages.Scripts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScriptRegistrations = ACore.Server.Modules.SettingModule.Storage.SQL.PG.Scripts.ScriptRegistrations;

namespace ACore.Server.Modules.SettingModule.Storage.SQL.PG;

using ScriptRegistrations = ScriptRegistrations;

internal class SettingModulePGStorageImpl : SettingModuleStorageImpl
{
  public override DbScriptBase UpdateScripts => new ScriptRegistrations();
  public override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Postgres);

  public SettingModulePGStorageImpl(DbContextOptions<SettingModulePGStorageImpl> options, IMediator mediator, ILogger<SettingModulePGStorageImpl> logger) : base(options, mediator, logger)
  {
  }

  public SettingModulePGStorageImpl(DbContextOptions<SettingModulePGStorageImpl> options, IMediator mediator, IAuditConfiguration auditConfiguration, ILogger<SettingModulePGStorageImpl> logger) : base(options, mediator, auditConfiguration, logger)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<SettingEntity>().HasKey(p => p.Id);
    
    SetDatabaseNames<SettingEntity>(modelBuilder);
  }
  
  private static void SetDatabaseNames<T>(ModelBuilder modelBuilder) where T : class => SetDatabaseNames<T>(DefaultNames.ObjectNameMapping, modelBuilder);
}