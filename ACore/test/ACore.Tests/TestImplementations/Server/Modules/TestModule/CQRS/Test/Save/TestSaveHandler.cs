using ACore.Base.CQRS.Models;
using ACore.Server.Storages;
using ACore.Server.Storages.CQRS;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.Test.Save;

internal class TestSaveHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestSaveCommand, Result>(storageResolver)
{
  public override async Task<Result> Handle(TestSaveCommand command, CancellationToken cancellationToken)
  {
    var allTask = new List<SaveHandlerData<TestEntity>>();
    foreach (var storage in WriteTestContexts())
    {
      if (storage is TestModuleSqlStorageImpl)
      {
        var en = TestEntity.Create(command.Data);
        allTask.Add(new SaveHandlerData<TestEntity>(en, storage, storage.Save<TestEntity, int>(en)));
      }
      else
        throw new Exception($"{nameof(TestSaveHandler)} cannot be used for storage {storage.GetType().Name}");
    }

    await Task.WhenAll(allTask.Select(e => e.Task));
    return DbSaveResult.SuccessWithPkValues(allTask.ToDictionary(k => k.Storage.StorageDefinition.Type, v => (object)v.Entity.Id));
  } 
}