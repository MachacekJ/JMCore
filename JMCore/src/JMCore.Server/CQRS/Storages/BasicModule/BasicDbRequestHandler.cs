using JMCore.Server.Configuration.Storage;
using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Storages.Modules.BasicModule;
using MediatR;

namespace JMCore.Server.CQRS.Storages.BasicModule
{
  public abstract class BasicDbRequestHandler<TRequest>(IStorageResolver storageResolver) : IRequestHandler<TRequest>
    where TRequest : IRequest
  {
    public abstract Task Handle(TRequest request, CancellationToken cancellationToken);

    protected IEnumerable<IBasicStorageModule> AllBasicStorageWriteContexts(StorageTypeEnum storageType) => storageResolver.AllStorageModuleImplementations<IBasicStorageModule>(storageType, StorageModeEnum.Write);
  }

  public abstract class BasicDbRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
  {
    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
  }
}