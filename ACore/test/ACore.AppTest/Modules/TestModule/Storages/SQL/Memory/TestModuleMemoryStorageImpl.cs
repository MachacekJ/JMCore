using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Storages.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ACore.AppTest.Modules.TestModule.Storages.SQL.Memory;

internal class TestModuleMemoryStorageImpl(DbContextOptions<TestModuleMemoryStorageImpl> options, IMediator mediator, ILogger<TestModuleSqlStorageImpl> logger, IAuditConfiguration auditConfiguration)
  : TestModuleSqlStorageImpl(options, mediator, logger, auditConfiguration)
{
  // public override Task<TEntity?> Get<TEntity, TPK>(TPK id) where TEntity : default
  // {
  //   throw new NotImplementedException();
  // }

  public override StorageTypeDefinition StorageDefinition => new(StorageTypeEnum.Memory);
  
  protected virtual int IdIntGenerator<T>()
  {
    return 1;
  }

  protected virtual long IdLongGenerator<T>()
  {
    return 1;
  }

  protected virtual string IdStringGenerator<T>()
  {
    return IdGuidGenerator<T>().ToString();
  }

  protected virtual Guid IdGuidGenerator<T>()
  {
    return Guid.NewGuid();
  }
}