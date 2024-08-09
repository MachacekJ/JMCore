using ACore.Base.CQRS.Results;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages;
using MediatR;

namespace ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS;

public abstract class TestModuleRequestHandler<TRequest, TResponse>(IStorageResolver storageResolver) : IRequestHandler<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
  where TResponse : Result
{
  public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
  protected ITestStorageModule ReadTestContext() => storageResolver.FirstReadOnlyStorage<ITestStorageModule>();
  protected IEnumerable<ITestStorageModule> WriteTestContexts() => storageResolver.WriteStorages<ITestStorageModule>();
}