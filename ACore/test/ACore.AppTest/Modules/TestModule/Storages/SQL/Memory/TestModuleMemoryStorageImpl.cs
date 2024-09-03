using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Storages.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.AppTest.Modules.TestModule.Storages.SQL.Memory;

internal class TestModuleMemoryStorageImpl(DbContextOptions<TestModuleMemoryStorageImpl> options, IMediator mediator, ILogger<TestModuleSqlStorageImpl> logger, IAuditConfiguration auditConfiguration)
  : TestModuleSqlStorageImpl(options, mediator, logger, auditConfiguration)
{
  public override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Memory);
}