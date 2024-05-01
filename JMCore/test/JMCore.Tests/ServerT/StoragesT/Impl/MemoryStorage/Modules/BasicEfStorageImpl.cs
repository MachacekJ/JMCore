using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Storages.Base.Audit.EF;
using JMCore.Server.Storages.Base.EF;
using JMCore.Server.Storages.Modules.BasicModule.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JMCore.Tests.ServerT.StoragesT.Impl.MemoryStorage.Modules;

public class BasicEfStorageImpl(DbContextOptions<BasicEfStorageImpl> options, IMediator mediator, IAuditDbService auditService, ILogger<BasicEfStorageImpl> logger)
  : BasicStorageEfContext(options, mediator, auditService, logger)
{
  public override DbScriptBase SqlScripts => new ScriptRegistrations();
  public override StorageTypeEnum StorageType => StorageTypeEnum.Memory;

  public BasicEfStorageImpl(DbContextOptions<BasicEfStorageImpl> options, IMediator mediator, ILogger<BasicEfStorageImpl> logger) : this(options, mediator, null, logger)
  {
  }
}