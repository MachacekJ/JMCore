using ACore.Server.Storages.Definitions.EF;
using ACore.Server.Storages.Definitions.EF.MemoryEFStorage;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Server.Modules.AuditModule.Storage.SQL.Memory;

internal class AuditSqlMemoryStorageImpl(DbContextOptions<AuditSqlMemoryStorageImpl> options, IMediator mediator, ILogger<AuditSqlMemoryStorageImpl> logger) : AuditSqlStorageImpl(options, mediator, logger)
{
  protected override EFStorageDefinition EFStorageDefinition => new MemoryEFStorageDefinition();
}