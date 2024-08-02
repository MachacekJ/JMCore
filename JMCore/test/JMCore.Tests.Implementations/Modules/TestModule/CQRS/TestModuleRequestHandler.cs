using JMCore.Server.Storages;
using JMCore.Tests.Implementations.Modules.TestModule.Storages;
using MediatR;

namespace JMCore.Tests.Implementations.Modules.TestModule.CQRS;

public abstract class TestModuleRequestHandler<TRequest, TResponse>(IStorageResolver storageResolver) : IRequestHandler<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
{
  public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
  protected IEnumerable<ITestStorageModule> AllTestStorageWriteContexts() => storageResolver.AllWriteStorages<ITestStorageModule>();
  protected ITestStorageModule ReadTestStorageWriteContexts() => storageResolver.FirstReadWriteStorage<ITestStorageModule>();
}