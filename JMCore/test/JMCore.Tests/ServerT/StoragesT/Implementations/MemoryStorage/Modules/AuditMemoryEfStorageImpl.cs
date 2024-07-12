using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Storages.Modules.AuditModule.EF;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JMCore.Tests.ServerT.StoragesT.Implemantations.MemoryStorage.Modules;

public class AuditMemoryEfStorageImpl(DbContextOptions<AuditMemoryEfStorageImpl> options, IMediator mediator, ILogger<AuditMemoryEfStorageImpl> logger) : AuditStorageEfContext(options, mediator, logger)
{
  public override StorageTypeEnum StorageType => StorageTypeEnum.Memory;
}