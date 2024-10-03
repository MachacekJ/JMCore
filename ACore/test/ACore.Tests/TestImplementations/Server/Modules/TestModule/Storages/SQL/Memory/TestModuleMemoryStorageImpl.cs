using ACore.Server.Storages.Definitions;
using ACore.Server.Storages.Definitions.EF;
using ACore.Server.Storages.Definitions.EF.MemoryEFStorage;
using ACore.Server.Storages.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Memory;

internal class TestModuleMemoryStorageImpl(DbContextOptions<TestModuleMemoryStorageImpl> options, IMediator mediator, ILogger<TestModuleSqlStorageImpl> logger)
  : TestModuleSqlStorageImpl(options, mediator, logger)
{
  protected override EFStorageDefinition EFStorageDefinition => new MemoryEFStorageDefinition();
}