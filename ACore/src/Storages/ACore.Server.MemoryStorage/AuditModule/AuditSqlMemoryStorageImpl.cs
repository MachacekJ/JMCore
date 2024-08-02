using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Storages.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.MemoryStorage.AuditModule;

public class AuditSqlMemoryStorageImpl(DbContextOptions<AuditSqlMemoryStorageImpl> options, IMediator mediator, ILogger<AuditSqlMemoryStorageImpl> logger) : AuditSqlStorageImpl(options, mediator, logger)
{
  public override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Memory);
}