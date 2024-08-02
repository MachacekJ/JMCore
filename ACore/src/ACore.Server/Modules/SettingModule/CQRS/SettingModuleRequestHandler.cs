using ACore.Server.Modules.SettingModule.Storage;
using ACore.Server.Storages;
using ACore.Server.Storages.Models;
using MediatR;

namespace ACore.Server.Modules.SettingModule.CQRS;

public abstract class SettingModuleRequestHandler<TRequest>(IStorageResolver storageResolver) : IRequestHandler<TRequest>
  where TRequest : IRequest
{
  public abstract Task Handle(TRequest request, CancellationToken cancellationToken);

  protected IEnumerable<IBasicStorageModule> AllBasicStorageWriteContexts(StorageTypeEnum storageType) => storageResolver.AllWriteStorages<IBasicStorageModule>(storageType);
}

public abstract class SettingModuleRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
{
  public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}