using ACore.Base.CQRS.Models.Results;
using ACore.Server.Storages;
using ACore.Server.Storages.CQRS;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestValueType.Save;

internal class TestValueTypeSaveHashHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestValueTypeSaveHashCommand, Result>(storageResolver)
{
  public override async Task<Result> Handle(TestValueTypeSaveHashCommand request, CancellationToken cancellationToken)
  {
    var allTask = new List<SaveHandlerData<TestValueTypeEntity>>();
    foreach (var storage in WriteTestContexts())
    {
      if (storage is TestModuleSqlStorageImpl)
      {
        var en = TestValueTypeEntity.Create(request.Data);
        allTask.Add(new SaveHandlerData<TestValueTypeEntity>(en, storage, storage.Save<TestValueTypeEntity, int>(en, request.Hash)));
      }
      else
        throw new Exception($"{nameof(TestValueTypeSaveHashHandler)} cannot be used for storage {storage.GetType().Name}");
    }

    await Task.WhenAll(allTask.Select(e => e.Task));
    return DbSaveResult.SuccessWithData(allTask);
  }
}