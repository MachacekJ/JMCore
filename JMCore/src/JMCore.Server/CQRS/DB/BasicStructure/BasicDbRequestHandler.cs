using JMCore.Server.Configuration.Storage;
using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Storages.Modules.BasicModule;
using MediatR;

namespace JMCore.Server.CQRS.DB.BasicStructure
{
  public abstract class BasicDbRequestHandler<TRequest>(IStorageResolver storageResolver) : IRequestHandler<TRequest>
    where TRequest : IRequest
  {
    public abstract Task Handle(TRequest request, CancellationToken cancellationToken);

    protected List<IBasicStorageModule> BasicDbWriteContexts(StorageTypeEnum storageType) => storageResolver.StorageModuleImplementations<IBasicStorageModule>( storageType,StorageModeEnum.Write);
  }

  public abstract class BasicDbRequestHandler<TRequest, TResponse>(IStorageResolver storageResolver) : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
  {
    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    private List<IBasicStorageModule> _basicDbContexts(StorageTypeEnum storageType) => storageResolver.StorageModuleImplementations<IBasicStorageModule>(storageType, StorageModeEnum.Read);

    protected IBasicStorageModule BasicDbReadContext(StorageTypeEnum storageType) => _basicDbContexts(storageType).Single();
  }
}