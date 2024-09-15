using ACore.Base.CQRS.Models;
using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Storages;
using MediatR;

namespace ACore.Server.Modules.AuditModule.CQRS;

public abstract class AuditModuleRequestHandler<TRequest, TResponse>(IStorageResolver storageResolver) : IRequestHandler<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
  public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
  protected IAuditStorageModule ReadAuditContext() => storageResolver.FirstReadOnlyStorage<IAuditStorageModule>();
  protected IEnumerable<IAuditStorageModule> WriteAuditContexts() => storageResolver.WriteStorages<IAuditStorageModule>();
}