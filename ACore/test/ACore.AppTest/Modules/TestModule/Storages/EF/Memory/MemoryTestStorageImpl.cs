using ACore.AppTest.Modules.TestModule.Storages.EF.Models;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.AuditModule.EF;
using ACore.Server.Storages.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.AppTest.Modules.TestModule.Storages.EF.Memory;

internal class MemoryTestStorageImpl(DbContextOptions<MemoryTestStorageImpl> options, IMediator mediator, ILogger<EFTestStorageContext> logger, IAuditDbService auditService, IAuditConfiguration auditConfiguration) 
  : EFTestStorageContext(options, mediator, logger, auditService, auditConfiguration)
{
  public override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Memory);

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<TestEntity>().Ignore(a => a.UId);
    modelBuilder.Entity<TestAttributeAuditEntity>().Ignore(a => a.UId);

    base.OnModelCreating(modelBuilder);
  }

  protected override int IdIntGenerator<T>()
  {
    return 1;
  }
  
  protected override long IdLongGenerator<T>()
  {
    return 1;
  }

  protected override string IdStringGenerator<T>()
  {
    return IdGuidGenerator<T>().ToString();
  }

  protected override Guid IdGuidGenerator<T>()
  {
    return Guid.NewGuid();
  }
}