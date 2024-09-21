using ACore.Base.CQRS.Models.Results;
using ACore.Server.Storages;
using ACore.Server.Storages.CQRS;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKLong.Save;

internal class TestPKLongSaveHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestPKLongSaveCommand, Result>(storageResolver)
{
  public override async Task<Result> Handle(TestPKLongSaveCommand request, CancellationToken cancellationToken)
  {
    var allTask = new List<SaveHandlerData<TestPKLongEntity>>();
    foreach (var storage in WriteTestContexts())
    {
      if (storage is TestModuleSqlStorageImpl)
      {
        var en = TestPKLongEntity.Create(request.Data);
        allTask.Add(new SaveHandlerData<TestPKLongEntity>(en, storage, storage.Save<TestPKLongEntity, long>(en)));
      }
      else
        throw new Exception($"{nameof(TestPKLongSaveHandler)} cannot be used for storage {storage.GetType().Name}");
    }

    await Task.WhenAll(allTask.Select(e => e.Task));
    return DbSaveResult.SuccessWithData(allTask);
  }
}