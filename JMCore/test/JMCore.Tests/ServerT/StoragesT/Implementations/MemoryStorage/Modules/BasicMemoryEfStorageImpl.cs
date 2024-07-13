using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Storages.Base.Audit.EF;
using JMCore.Server.Storages.Base.EF;
using JMCore.Server.Storages.Modules.BasicModule.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JMCore.Tests.ServerT.StoragesT.Implementations.MemoryStorage.Modules;

public class BasicMemoryEfStorageImpl(DbContextOptions<BasicMemoryEfStorageImpl> options, IMediator mediator, IAuditDbService? auditService, ILogger<BasicMemoryEfStorageImpl> logger)
  : BasicStorageEfContext(options, mediator, auditService, logger)
{
  public override DbScriptBase UpdateScripts => new ScriptRegistrations();
  protected override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Memory);

  public BasicMemoryEfStorageImpl(DbContextOptions<BasicMemoryEfStorageImpl> options, IMediator mediator, ILogger<BasicMemoryEfStorageImpl> logger) : this(options, mediator, null, logger)
  {
  }
}