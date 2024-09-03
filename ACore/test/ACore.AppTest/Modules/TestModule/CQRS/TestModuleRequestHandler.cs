using ACore.AppTest.Modules.TestModule.Storages;
using ACore.CQRS;
using ACore.Models;
using ACore.Server.Storages;
using MediatR;

namespace ACore.AppTest.Modules.TestModule.CQRS;

public abstract class TestModuleRequestHandler<TRequest>(IStorageResolver storageResolver) : TestModuleRequestHandlerBase(storageResolver), ICQRSRequestHandler<TRequest>
  where TRequest : IResultRequest
{
  public abstract Task<Result> Handle(TRequest request, CancellationToken cancellationToken);
}

public abstract class TestModuleRequestHandler<TRequest, TResponse>(IStorageResolver storageResolver) : TestModuleRequestHandlerBase(storageResolver), ICQRSRequestHandler<TRequest, TResponse>
  where TRequest : IResultRequest<TResponse>
{
  public abstract Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);
}

public abstract class TestModuleRequestHandlerBase(IStorageResolver storageResolver)
{
  protected readonly Exception NoStorageException = new Exception($"Operation failed. No '{nameof(ITestStorageModule)}' is registered.");
  protected IEnumerable<ITestStorageModule> WriteStorages()
  {
    var storages = storageResolver.WriteStorages<ITestStorageModule>().ToArray();
    if (storages.Length == 0)
      throw NoStorageException;
    return storages;
  } 
  protected ITestStorageModule ReadTestStorageWriteContexts() => storageResolver.FirstReadOnlyStorage<ITestStorageModule>();
}