using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Storages.Base.Audit.EF;
using JMCore.Tests.ServerT.StoragesT.Impl.TestStorageModule;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JMCore.Tests.ServerT.StoragesT.Impl.MemoryStorage.Modules;

public class TestStorageEfStorageImpl(DbContextOptions options, IMediator mediator, ILogger<TestStorageEfContext> logger, IAuditDbService auditService) : TestStorageEfContext(options, mediator, logger, auditService)
{
  public override StorageTypeEnum StorageType => StorageTypeEnum.Memory;
}