using ACore.AppTest.Modules.TestModule.Storages.EF;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Storages.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.AppTest.Modules.TestModule.Storages.Memory;

internal class MemoryTestStorageImpl(DbContextOptions<MemoryTestStorageImpl> options, IMediator mediator, ILogger<EFTestStorageContext> logger, IAuditConfiguration auditConfiguration)
  : EFTestStorageContext(options, mediator, logger, auditConfiguration)
{
  public override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Memory);
  
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