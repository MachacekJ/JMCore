using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Storages.Modules.AuditModule.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JMCore.Tests.ServerT.StoragesT.Impl.MemoryStorage.Modules;

public class AuditEfStorageImpl(DbContextOptions<AuditEfStorageImpl> options, IMediator mediator, ILogger<AuditEfStorageImpl> logger) : AuditStorageEfContext(options, mediator, logger)
{
  public override StorageTypeEnum StorageType => StorageTypeEnum.Memory;
}