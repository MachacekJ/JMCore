using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Storages.Base.Audit.EF;
using JMCore.Tests.ServerT.StoragesT.Impl.TestStorageModule;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JMCore.Tests.ServerT.StoragesT.Impl.MemoryStorage.Modules;

public class TestStorageEfEfContextEfContextEfContext(DbContextOptions<TestStorageEfEfContextEfContextEfContext> options, IMediator mediator, ILogger<TestStorageModule.TestStorageEfContext> logger, IAuditDbService auditService)
  : TestStorageModule.TestStorageEfContext(options, mediator, logger, auditService)
{
  public override StorageTypeEnum StorageType { get; } = StorageTypeEnum.Memory;
}