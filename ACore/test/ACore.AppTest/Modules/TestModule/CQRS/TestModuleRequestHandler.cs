﻿using ACore.AppTest.Modules.TestModule.Storages;
using ACore.Server.Storages;
using MediatR;

namespace ACore.AppTest.Modules.TestModule.CQRS;

internal abstract class TestModuleRequestHandler<TRequest, TResponse>(IStorageResolver storageResolver) : IRequestHandler<TRequest, TResponse>
  where TRequest : IRequest<TResponse>
{
  public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
  protected IEnumerable<ITestStorageModule> AllTestStorageWriteContexts() => storageResolver.AllWriteStorages<ITestStorageModule>();
  protected ITestStorageModule ReadTestStorageWriteContexts() => storageResolver.FirstReadWriteStorage<ITestStorageModule>();
}