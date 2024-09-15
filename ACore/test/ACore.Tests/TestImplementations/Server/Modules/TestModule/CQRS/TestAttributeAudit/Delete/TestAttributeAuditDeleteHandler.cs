using ACore.Base.CQRS.Models;
using ACore.Server.Storages;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.Mongo;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.Mongo.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAttributeAudit.Delete;

public class TestAttributeAuditDeleteHandler<T>(IStorageResolver storageResolver) 
  : TestModuleRequestHandler<TestAttributeAuditDeleteCommand<T>, Result>(storageResolver)
  where T : IConvertible

{
  public override async Task<Result> Handle(TestAttributeAuditDeleteCommand<T> request, CancellationToken cancellationToken)
  {
    var allTask = new List<Task>();
    foreach (var storage in WriteTestContexts())
    {
      switch (storage)
      {
        case TestModuleMongoStorageImpl:
          var t = storage.Delete<TestAttributeAuditPKMongoEntity, T>(request.Id);
          allTask.Add(t);
          break;
        default:
          var t2 = storage.Delete<TestAttributeAuditPKIntEntity, T>(request.Id);
          allTask.Add(t2);
          break;
      }
    }
    await Task.WhenAll(allTask);
    return Result.Success();
  }
}