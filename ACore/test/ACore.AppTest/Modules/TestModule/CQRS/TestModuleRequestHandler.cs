using ACore.AppTest.Modules.TestModule.Storages;
using ACore.Server.Storages;
using MediatR;

namespace ACore.AppTest.Modules.TestModule.CQRS;

public abstract class TestModuleRequestHandler<TRequest, TResponse>(IStorageResolver storageResolver) : IRequestHandler<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
{
  public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
  protected ITestStorageModule WriteStorage() => storageResolver.AllWriteStorages<ITestStorageModule>().Single();
  protected ITestStorageModule ReadTestStorageWriteContexts() => storageResolver.FirstReadOnlyStorage<ITestStorageModule>();
}