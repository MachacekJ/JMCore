using ACore.Base.CQRS.Models;
using ACore.Server.Storages;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKGuid.Save;

internal class TestPKGuidSaveHandler(IStorageResolver storageResolver) 
  : TestModuleRequestHandler<TestPKGuidSaveCommand, Result>(storageResolver)
{
  public override async Task<Result> Handle(TestPKGuidSaveCommand request, CancellationToken cancellationToken)
  {
    var allTask = new List<Task>();
    foreach (var storage in WriteTestContexts())
    {
      var en = TestPKGuidEntity.Create(request.Data);
      allTask.Add(storage.Save<TestPKGuidEntity, Guid>(en));
    }

    await Task.WhenAll(allTask);
    return Result.Success();
  }
}