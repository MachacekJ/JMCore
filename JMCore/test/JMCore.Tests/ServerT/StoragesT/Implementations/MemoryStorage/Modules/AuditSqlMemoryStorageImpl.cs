using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Storages.Modules.AuditModule.BaseImpl;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JMCore.Tests.ServerT.StoragesT.Implementations.MemoryStorage.Modules;

public class AuditSqlMemoryStorageImpl(DbContextOptions<AuditSqlMemoryStorageImpl> options, IMediator mediator, ILogger<AuditSqlMemoryStorageImpl> logger) : AuditSqlStorageImpl(options, mediator, logger)
{
  protected override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Memory);
}