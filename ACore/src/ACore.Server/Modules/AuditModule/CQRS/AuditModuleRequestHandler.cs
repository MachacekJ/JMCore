using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Storages;
using MediatR;

namespace ACore.Server.Modules.AuditModule.CQRS;

internal abstract class AuditModuleRequestHandler<TRequest, TResponse>(IStorageResolver storageResolver) : IRequestHandler<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
{
  public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken); 
  protected IAuditStorageModule ReadAuditContexts() => storageResolver.FirstReadWriteStorage<IAuditStorageModule>();
}