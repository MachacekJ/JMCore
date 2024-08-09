using ACore.Server.Modules.SettingsDbModule.Storage.SQL.Models;
using ACore.Server.Storages.Definitions.EF;
using ACore.Server.Storages.Definitions.EF.Base.Scripts;
using ACore.Server.Storages.Definitions.EF.MemoryEFStorage;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Modules.SettingsDbModule.Storage.SQL.Memory;

internal class SettingsDbModuleSqlMemoryStorageImplTestsSqlMemoryStorageImpl(DbContextOptions<SettingsDbModuleSqlMemoryStorageImplTestsSqlMemoryStorageImpl> options, IMediator mediator, ILogger<SettingsDbModuleSqlMemoryStorageImplTestsSqlMemoryStorageImpl> logger)
  : SettingsDbModuleSqlStorageImpl(options, mediator, logger)
{
  protected override DbScriptBase UpdateScripts => new ScriptRegistrations();
  protected override EFStorageDefinition EFStorageDefinition => new MemoryEFStorageDefinition();
  
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<SettingsEntity>().HasKey(p => p.Key);
  }
}