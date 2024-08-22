using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.SettingModule.Storage.BaseImpl;
using ACore.Server.Storages.EF;
using ACore.Server.Storages.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Modules.SettingModule.Storage.Memory;

internal class BasicSqlMemoryEfStorageImpl(DbContextOptions<BasicSqlMemoryEfStorageImpl> options, IMediator mediator, IAuditConfiguration? auditConfiguration, ILogger<BasicSqlMemoryEfStorageImpl> logger)
  : BasicSqlStorageImpl(options, mediator, auditConfiguration, logger)
{
  public override DbScriptBase UpdateScripts => new ScriptRegistrations();
  public override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Memory);

  public BasicSqlMemoryEfStorageImpl(DbContextOptions<BasicSqlMemoryEfStorageImpl> options, IMediator mediator, ILogger<BasicSqlMemoryEfStorageImpl> logger) : this(options, mediator, null, logger)
  {
  }
}