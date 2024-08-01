using JMCore.Server.Modules.SettingModule.Storage;
using JMCore.Server.Storages;
using JMCore.Server.Storages.Configuration;
using JMCore.Server.Storages.Models;
using MediatR;

namespace JMCore.Server.Modules.SettingModule.CQRS
{
  public abstract class SettingModuleRequestHandler<TRequest>(IStorageResolver storageResolver) : IRequestHandler<TRequest>
    where TRequest : IRequest
  {
    public abstract Task Handle(TRequest request, CancellationToken cancellationToken);

    protected IEnumerable<IBasicStorageModule> AllBasicStorageWriteContexts(StorageTypeEnum storageType) => storageResolver.AllStorageModuleImplementations<IBasicStorageModule>(storageType, StorageModeEnum.Write);
  }

  public abstract class SettingModuleRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
  {
    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
  }
}