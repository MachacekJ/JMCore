using ACore.Base.CQRS.Models.Results;
using ACore.Server.Storages;
using ACore.Server.Storages.CQRS;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestValueType.Save;

internal class TestValueTypeSaveHashHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestValueTypeSaveCommand, Result>(storageResolver)
{
  public override async Task<Result> Handle(TestValueTypeSaveCommand request, CancellationToken cancellationToken)
  {
    var allTask = new List<SavingProcessData<TestValueTypeEntity>>();
    foreach (var storage in WriteTestContexts())
    {
      if (storage is TestModuleSqlStorageImpl)
      {
        var en = TestValueTypeEntity.Create(request.Data);
        allTask.Add(new SavingProcessData<TestValueTypeEntity>(en, storage, storage.SaveTestEntity<TestValueTypeEntity, int>(en, request.Hash)));
      }
      else
        throw new Exception($"{nameof(TestValueTypeSaveHashHandler)} cannot be used for storage {storage.GetType().Name}");
    }

    await Task.WhenAll(allTask.Select(e => e.Task));
    return DbSaveResult.SuccessWithData(allTask);
  }
}