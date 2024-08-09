using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.AuditModule.EF;
using ACore.Server.Modules.SettingModule.Storage.BaseImpl;
using ACore.Server.Storages.EF;
using ACore.Server.Storages.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.MemoryStorage.SettingModule;

public class BasicSqlMemoryEfStorageImpl(DbContextOptions<BasicSqlMemoryEfStorageImpl> options, IMediator mediator, IAuditDbService? auditService, IAuditConfiguration? auditConfiguration, ILogger<BasicSqlMemoryEfStorageImpl> logger)
  : BasicSqlStorageImpl(options, mediator, auditService, auditConfiguration, logger)
{
  public override DbScriptBase UpdateScripts => new ScriptRegistrations();
  public override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Memory);

  public BasicSqlMemoryEfStorageImpl(DbContextOptions<BasicSqlMemoryEfStorageImpl> options, IMediator mediator, ILogger<BasicSqlMemoryEfStorageImpl> logger) : this(options, mediator, null, null, logger)
  {
  }
}