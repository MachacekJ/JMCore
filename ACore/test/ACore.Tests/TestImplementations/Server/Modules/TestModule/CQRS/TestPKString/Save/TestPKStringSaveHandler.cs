using ACore.Base.CQRS.Models;
using ACore.Server.Storages;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKString.Save;

internal class TestPKStringSaveHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestPKStringSaveCommand, Result>(storageResolver)
{
  public override async Task<Result> Handle(TestPKStringSaveCommand request, CancellationToken cancellationToken)
  {
    var allTask = new List<Task>();
    foreach (var storage in WriteTestContexts())
    {
      var en = TestPKStringEntity.Create(request.Data);
      allTask.Add(storage.Save<TestPKStringEntity, string>(en));
    }

    await Task.WhenAll(allTask);
    return Result.Success();

  }
}