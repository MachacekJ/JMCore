using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.AuditModule.EF;
using ACore.Server.Storages.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Modules.SettingModule.Storage.BaseImpl;

public abstract class BasicSqlStorageImpl(DbContextOptions options, IMediator mediator, IAuditDbService? auditService, IAuditConfiguration? auditConfiguration, ILogger<BasicSqlStorageImpl> logger)
  : AuditableDbContext(options, mediator, logger, auditService, auditConfiguration)
{
  protected override string ModuleName => nameof(IBasicStorageModule);

  public override Task<TEntity?> Get<TEntity, TPK>(TPK id) where TEntity : class
  {
    throw new NotImplementedException();
  }

  protected BasicSqlStorageImpl(DbContextOptions options, IMediator mediator, ILogger<BasicSqlStorageImpl> logger) : this(options, mediator, null, null, logger)
  {
  }
}