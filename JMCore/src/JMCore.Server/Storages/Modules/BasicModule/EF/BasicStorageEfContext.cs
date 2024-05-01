using JMCore.Server.Storages.Base.Audit.EF;
using JMCore.Server.Storages.Base.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JMCore.Server.Storages.Modules.BasicModule.EF;

public abstract class BasicStorageEfContext(DbContextOptions options, IMediator mediator, IAuditDbService? auditService, ILogger<BasicStorageEfContext> logger)
  : DbContextBase(options, mediator, logger, auditService)
{
  public override string ModuleName => nameof(IBasicStorageModule);

  protected BasicStorageEfContext(DbContextOptions options, IMediator mediator, ILogger<BasicStorageEfContext> logger) : this(options, mediator, null, logger)
  {
  }
}