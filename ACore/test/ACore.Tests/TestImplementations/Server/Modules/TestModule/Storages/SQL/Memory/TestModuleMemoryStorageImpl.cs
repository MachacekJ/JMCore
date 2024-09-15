using ACore.Server.Storages.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Memory;

internal class TestModuleMemoryStorageImpl(DbContextOptions<TestModuleMemoryStorageImpl> options, IMediator mediator, ILogger<TestModuleSqlStorageImpl> logger)
  : TestModuleSqlStorageImpl(options, mediator, logger)
{
  public override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Memory);
}