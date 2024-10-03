using ACore.Server.Modules.SettingsDbModule.Storage.SQL.Models;
using ACore.Server.Storages.Definitions;
using ACore.Server.Storages.Definitions.EF;
using ACore.Server.Storages.Definitions.EF.Base.Scripts;
using ACore.Server.Storages.Definitions.EF.PGStorage;
using ACore.Server.Storages.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScriptRegistrations = ACore.Server.Modules.SettingsDbModule.Storage.SQL.PG.Scripts.ScriptRegistrations;

namespace ACore.Server.Modules.SettingsDbModule.Storage.SQL.PG;

using ScriptRegistrations = ScriptRegistrations;

internal class SettingsDbModuleSqlPGStorageImpl(DbContextOptions<SettingsDbModuleSqlPGStorageImpl> options, IMediator mediator, ILogger<SettingsDbModuleSqlPGStorageImpl> logger) : SettingsDbModuleSqlStorageImpl(options, mediator, logger)
{
  protected override DbScriptBase UpdateScripts => new ScriptRegistrations();
  protected override EFStorageDefinition EFStorageDefinition => new PGStorageDefinition();
  
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<SettingsEntity>().HasKey(p => p.Id);
    
    SetDatabaseNames<SettingsEntity>(modelBuilder);
  }
  
  private static void SetDatabaseNames<T>(ModelBuilder modelBuilder) where T : class => SetDatabaseNames<T>(DefaultNames.ObjectNameMapping, modelBuilder);
}