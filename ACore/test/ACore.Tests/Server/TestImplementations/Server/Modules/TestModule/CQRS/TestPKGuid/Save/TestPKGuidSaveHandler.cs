﻿using ACore.Base.CQRS.Results;
using ACore.Server.Storages.CQRS;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.SQL;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

namespace ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestPKGuid.Save;

internal class TestPKGuidSaveHandler(IStorageResolver storageResolver) 
  : TestModuleRequestHandler<TestPKGuidSaveCommand, Result>(storageResolver)
{
  public override async Task<Result> Handle(TestPKGuidSaveCommand request, CancellationToken cancellationToken)
  {
    var allTask = new List<SaveProcessExecutor<TestPKGuidEntity>>();
    foreach (var storage in WriteTestContexts())
    {
      if (storage is TestModuleSqlStorageImpl)
      {
        var en = TestPKGuidEntity.Create(request.Data);
        allTask.Add(new SaveProcessExecutor<TestPKGuidEntity>(en, storage, storage.SaveTestEntity<TestPKGuidEntity, Guid>(en)));
      }
      else
        throw new Exception($"{nameof(TestPKGuidSaveHandler)} cannot be used for storage {storage.GetType().Name}");
    }

    await Task.WhenAll(allTask.Select(e => e.Task));
    return DbSaveResult.SuccessWithData(allTask);
  }
}