using ACore.CQRS;
using ACore.Models;
using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Storages;

namespace ACore.Server.Modules.AuditModule.CQRS;

public abstract class AuditModuleRequestHandler<TRequest>(IStorageResolver storageResolver) : ICQRSRequestHandler<TRequest>
  where TRequest : IResultRequest
{
  public abstract Task<Result> Handle(TRequest request, CancellationToken cancellationToken);

  protected IEnumerable<IAuditStorageModule> AllBasicStorageWriteContexts() => storageResolver.WriteStorages<IAuditStorageModule>();
}

public abstract class AuditModuleRequestHandler<TRequest, TResponse>(IStorageResolver storageResolver) : ICQRSRequestHandler<TRequest, TResponse>
  where TRequest : IResultRequest<TResponse>
{
  public abstract Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken); 
  protected IAuditStorageModule ReadAuditContexts() => storageResolver.FirstReadOnlyStorage<IAuditStorageModule>();
}