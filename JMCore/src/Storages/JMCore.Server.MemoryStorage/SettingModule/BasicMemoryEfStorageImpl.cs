using JMCore.Server.Modules.AuditModule.EF;
using JMCore.Server.Modules.SettingModule.Storage.BaseImpl;
using JMCore.Server.Storages.EF;
using JMCore.Server.Storages.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JMCore.Server.MemoryStorage.SettingModule;

public class BasicSqlMemoryEfStorageImpl(DbContextOptions<BasicSqlMemoryEfStorageImpl> options, IMediator mediator, IAuditDbService? auditService, ILogger<BasicSqlMemoryEfStorageImpl> logger)
  : BasicSqlStorageImpl(options, mediator, auditService, logger)
{
  public override DbScriptBase UpdateScripts => new ScriptRegistrations();
  public override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Memory);

  public BasicSqlMemoryEfStorageImpl(DbContextOptions<BasicSqlMemoryEfStorageImpl> options, IMediator mediator, ILogger<BasicSqlMemoryEfStorageImpl> logger) : this(options, mediator, null, logger)
  {
  }
}