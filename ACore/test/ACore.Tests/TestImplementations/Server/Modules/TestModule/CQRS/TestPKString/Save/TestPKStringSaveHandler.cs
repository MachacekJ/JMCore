﻿using ACore.Base.CQRS.Models.Results;
using ACore.Server.Storages;
using ACore.Server.Storages.CQRS;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKString.Save;

internal class TestPKStringSaveHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestPKStringSaveCommand, Result>(storageResolver)
{
  public override async Task<Result> Handle(TestPKStringSaveCommand request, CancellationToken cancellationToken)
  {
    var allTask = new List<SavingProcessData<TestPKStringEntity>>();
    foreach (var storage in WriteTestContexts())
    {
      if (storage is TestModuleSqlStorageImpl)
      {
        var en = TestPKStringEntity.Create(request.Data);
        allTask.Add(new SavingProcessData<TestPKStringEntity>(en, storage, storage.SaveTestEntity<TestPKStringEntity, string>(en)));
      }
      else
        throw new Exception($"{nameof(TestPKStringSaveHandler)} cannot be used for storage {storage.GetType().Name}");
    }

    await Task.WhenAll(allTask.Select(e => e.Task));
    return DbSaveResult.SuccessWithData(allTask);
  }
}