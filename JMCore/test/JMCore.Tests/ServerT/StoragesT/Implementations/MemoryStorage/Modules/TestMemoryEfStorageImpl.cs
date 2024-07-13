using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Storages.Base.Audit.EF;
using JMCore.Tests.ServerT.StoragesT.Implementations.TestStorageModule;
using JMCore.Tests.ServerT.StoragesT.Implementations.TestStorageModule.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JMCore.Tests.ServerT.StoragesT.Implementations.MemoryStorage.Modules;

public class TestMemoryEfStorageImpl(DbContextOptions<TestMemoryEfStorageImpl> options, IMediator mediator, ILogger<TestStorageEfContext> logger, IAuditDbService auditService)
  : TestStorageEfContext(options, mediator, logger, auditService)
{
  protected override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Memory);

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<TestEntity>().Ignore(a => a.UId);
    modelBuilder.Entity<TestAttributeAuditEntity>().Ignore(a => a.UId);

    base.OnModelCreating(modelBuilder);
  }
}