using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Storages.Base.Audit.EF;
using JMCore.Server.Storages.Base.EF;
using JMCore.Server.Storages.Modules.BasicModule.BaseImpl;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JMCore.Tests.ServerT.StoragesT.Implementations.MemoryStorage.Modules;

public class BasicSqlMemoryEfStorageImpl(DbContextOptions<BasicSqlMemoryEfStorageImpl> options, IMediator mediator, IAuditDbService? auditService, ILogger<BasicSqlMemoryEfStorageImpl> logger)
  : BasicSqlStorageImpl(options, mediator, auditService, logger)
{
  public override DbScriptBase UpdateScripts => new ScriptRegistrations();
  protected override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Memory);

  public BasicSqlMemoryEfStorageImpl(DbContextOptions<BasicSqlMemoryEfStorageImpl> options, IMediator mediator, ILogger<BasicSqlMemoryEfStorageImpl> logger) : this(options, mediator, null, logger)
  {
  }
}