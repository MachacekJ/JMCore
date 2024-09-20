using ACore.Base.CQRS.Models;
using ACore.Base.CQRS.Models.Results;
using ACore.Server.Storages;
using ACore.Server.Storages.CQRS;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestNoAudit.Save;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKGuid.Save;

internal class TestPKGuidSaveHandler(IStorageResolver storageResolver) 
  : TestModuleRequestHandler<TestPKGuidSaveCommand, Result>(storageResolver)
{
  public override async Task<Result> Handle(TestPKGuidSaveCommand request, CancellationToken cancellationToken)
  {
    var allTask = new List<SaveHandlerData<TestPKGuidEntity>>();
    foreach (var storage in WriteTestContexts())
    {
      if (storage is TestModuleSqlStorageImpl)
      {
        var en = TestPKGuidEntity.Create(request.Data);
        allTask.Add(new SaveHandlerData<TestPKGuidEntity>(en, storage, storage.Save<TestPKGuidEntity, Guid>(en)));
      }
      else
        throw new Exception($"{nameof(TestPKGuidSaveHandler)} cannot be used for storage {storage.GetType().Name}");
    }

    await Task.WhenAll(allTask.Select(e => e.Task));
    return DbSaveResult.SuccessWithData(allTask);
  }
}