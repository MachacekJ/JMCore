using System.Runtime.ExceptionServices;
using ACore.Base.CQRS.Results;
using ACore.Server.Storages.CQRS;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.SQL;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

namespace ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestValueType.Save;

internal class TestValueTypeSaveHashHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestValueTypeSaveCommand, Result>(storageResolver)
{
  public override async Task<Result> Handle(TestValueTypeSaveCommand request, CancellationToken cancellationToken)
  {
    var allTask = new List<SaveProcessExecutor<TestValueTypeEntity>>();
    foreach (var storage in WriteTestContexts())
    {
      if (storage is TestModuleSqlStorageImpl)
      {
        var en = TestValueTypeEntity.Create(request.Data);
        allTask.Add(new SaveProcessExecutor<TestValueTypeEntity>(en, storage, storage.SaveTestEntity<TestValueTypeEntity, int>(en, request.Hash)));
      }
      else
        throw new Exception($"{nameof(TestValueTypeSaveHashHandler)} cannot be used for storage {storage.GetType().Name}");
    }

    Task? taskSum = null;
    try
    {
      taskSum = Task.WhenAll(allTask.Select(e => e.Task));
      await taskSum.ConfigureAwait(false);
    }
    catch
    {
      if (taskSum?.Exception != null) ExceptionDispatchInfo.Capture(taskSum.Exception).Throw();
      throw;
    }
    
    return DbSaveResult.SuccessWithData(allTask);
  }
}