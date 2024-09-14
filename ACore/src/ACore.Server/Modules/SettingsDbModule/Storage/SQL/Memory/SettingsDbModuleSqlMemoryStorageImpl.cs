using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.SettingsDbModule.Storage.SQL.Models;
using ACore.Server.Storages.Models;
using ACore.Server.Storages.Scripts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Modules.SettingsDbModule.Storage.SQL.Memory;

internal class SettingsDbModuleSqlMemoryStorageImpl(DbContextOptions<SettingsDbModuleSqlMemoryStorageImpl> options, IMediator mediator, IAuditConfiguration? auditConfiguration, ILogger<SettingsDbModuleSqlMemoryStorageImpl> logger)
  : SettingsDbModuleSqlStorageImpl(options, mediator, auditConfiguration, logger)
{
  public override DbScriptBase UpdateScripts => new ScriptRegistrations();
  public override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Memory);

  public SettingsDbModuleSqlMemoryStorageImpl(DbContextOptions<SettingsDbModuleSqlMemoryStorageImpl> options, IMediator mediator, ILogger<SettingsDbModuleSqlMemoryStorageImpl> logger) : this(options, mediator, null, logger)
  {
  }
  
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<SettingsEntity>().HasKey(p => p.Key);
  }
}