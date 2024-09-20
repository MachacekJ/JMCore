using ACore.Server.Modules.SettingsDbModule.Storage.SQL.Models;
using ACore.Server.Storages.Models;
using ACore.Server.Storages.Scripts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Modules.SettingsDbModule.Storage.SQL.Memory;

internal class SettingsDbModuleSqlMemoryStorageImplTestsSqlMemoryStorageImpl(DbContextOptions<SettingsDbModuleSqlMemoryStorageImplTestsSqlMemoryStorageImpl> options, IMediator mediator, ILogger<SettingsDbModuleSqlMemoryStorageImplTestsSqlMemoryStorageImpl> logger)
  : SettingsDbModuleSqlStorageImpl(options, mediator, logger)
{
  public override DbScriptBase UpdateScripts => new ScriptRegistrations();
  public override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Memory);
  
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<SettingsEntity>().HasKey(p => p.Key);
  }
}