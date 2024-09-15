using ACore.Base.CQRS.Models;
using ACore.Server.Storages;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestValueType.Save;

internal class TestValueTypeSaveHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestValueTypeSaveCommand, Result>(storageResolver)
{
  public override async Task<Result> Handle(TestValueTypeSaveCommand request, CancellationToken cancellationToken)
  {
    var task = new List<Task>();
    foreach (var storage in WriteTestContexts())
    {
      var en = TestValueTypeEntity.Create(request.Data);
      task.Add(storage.Save<TestValueTypeEntity, int>(en));
    }

    await Task.WhenAll(task);
    return Result.Success();
  }
}