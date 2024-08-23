using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.SettingModule.Storage.SQL.Models;
using ACore.Server.Storages.Models;
using ACore.Server.Storages.Scripts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Modules.SettingModule.Storage.SQL.Memory;

internal class SettingModuleMemoryStorageImpl(DbContextOptions<SettingModuleMemoryStorageImpl> options, IMediator mediator, IAuditConfiguration? auditConfiguration, ILogger<SettingModuleMemoryStorageImpl> logger)
  : SettingModuleStorageImpl(options, mediator, auditConfiguration, logger)
{
  public override DbScriptBase UpdateScripts => new ScriptRegistrations();
  public override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Memory);

  public SettingModuleMemoryStorageImpl(DbContextOptions<SettingModuleMemoryStorageImpl> options, IMediator mediator, ILogger<SettingModuleMemoryStorageImpl> logger) : this(options, mediator, null, logger)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<SettingEntity>().HasKey(p => p.Key);
  }
}