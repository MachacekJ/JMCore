using ACore.AppTest.Modules.TestModule.CQRS.Test.Models;
using ACore.AppTest.Modules.TestModule.Storages.SQL.Models;
using ACore.Models;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.Test.Save;

internal class TestSaveHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestSaveCommand, TestData>(storageResolver)
{
  public override async Task<Result<TestData>> Handle(TestSaveCommand command, CancellationToken cancellationToken)
  {
    var task = new List<Task>();
    TestEntity? first = null;
    foreach (var storage in WriteStorages())
    {
      var en = TestEntity.Create(command.Data);
      task.Add(storage.Save<TestEntity, int>(en));
      first ??= en;
    }
    await Task.WhenAll(task);
    if (first != null) 
      return Result.Success(first.ToData());

    throw NoStorageException;
  }
}