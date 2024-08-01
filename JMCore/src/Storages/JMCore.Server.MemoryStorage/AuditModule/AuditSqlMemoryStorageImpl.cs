using JMCore.Server.Modules.AuditModule.Storage;
using JMCore.Server.Storages.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JMCore.Server.MemoryStorage.AuditModule;

public class AuditSqlMemoryStorageImpl(DbContextOptions<AuditSqlMemoryStorageImpl> options, IMediator mediator, ILogger<AuditSqlMemoryStorageImpl> logger) : AuditSqlStorageImpl(options, mediator, logger)
{
  protected override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Memory);
}