using ACore.Server.Modules.AuditModule.EF;
using ACore.Server.Storages.Models;
using ACore.Tests.Implementations.Modules.TestModule.Storages.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.Tests.Implementations.Modules.TestModule.Storages.Memory;

internal class TestMemoryEfStorageImpl(DbContextOptions<TestMemoryEfStorageImpl> options, IMediator mediator, ILogger<TestStorageEfContext> logger, IAuditDbService auditService)
  : TestStorageEfContext(options, mediator, logger, auditService)
{
  public override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Memory);

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<TestEntity>().Ignore(a => a.UId);
    modelBuilder.Entity<TestAttributeAuditEntity>().Ignore(a => a.UId);

    base.OnModelCreating(modelBuilder);
  }
}