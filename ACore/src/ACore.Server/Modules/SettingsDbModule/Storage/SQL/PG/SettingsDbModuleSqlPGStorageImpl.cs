using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.SettingsDbModule.Storage.SQL.Models;
using ACore.Server.Storages.Models;
using ACore.Server.Storages.Scripts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScriptRegistrations = ACore.Server.Modules.SettingsDbModule.Storage.SQL.PG.Scripts.ScriptRegistrations;

namespace ACore.Server.Modules.SettingsDbModule.Storage.SQL.PG;

using ScriptRegistrations = ScriptRegistrations;

internal class SettingsDbModuleSqlPGStorageImpl : SettingsDbModuleSqlStorageImpl
{
  public override DbScriptBase UpdateScripts => new ScriptRegistrations();
  public override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Postgres);

  public SettingsDbModuleSqlPGStorageImpl(DbContextOptions<SettingsDbModuleSqlPGStorageImpl> options, IMediator mediator, ILogger<SettingsDbModuleSqlPGStorageImpl> logger) : base(options, mediator, logger)
  {
  }

  public SettingsDbModuleSqlPGStorageImpl(DbContextOptions<SettingsDbModuleSqlPGStorageImpl> options, IMediator mediator, IAuditConfiguration auditConfiguration, ILogger<SettingsDbModuleSqlPGStorageImpl> logger) : base(options, mediator, auditConfiguration, logger)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<SettingsEntity>().HasKey(p => p.Id);
    
    SetDatabaseNames<SettingsEntity>(modelBuilder);
  }
  
  private static void SetDatabaseNames<T>(ModelBuilder modelBuilder) where T : class => SetDatabaseNames<T>(DefaultNames.ObjectNameMapping, modelBuilder);
}