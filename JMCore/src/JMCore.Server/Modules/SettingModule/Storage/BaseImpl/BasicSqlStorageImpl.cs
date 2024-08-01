using JMCore.Server.Modules.AuditModule.EF;
using JMCore.Server.Storages.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JMCore.Server.Modules.SettingModule.Storage.BaseImpl;

public abstract class BasicSqlStorageImpl(DbContextOptions options, IMediator mediator, IAuditDbService? auditService, ILogger<BasicSqlStorageImpl> logger)
  : DbContextBase(options, mediator, logger, auditService)
{
  protected override string ModuleName => nameof(IBasicStorageModule);

  protected BasicSqlStorageImpl(DbContextOptions options, IMediator mediator, ILogger<BasicSqlStorageImpl> logger) : this(options, mediator, null, logger)
  {
  }
}