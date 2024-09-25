using ACore.Base.CQRS.Models.Results;
using ACore.Server.Storages;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.Mongo;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.Mongo.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using MongoDB.Bson;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Delete;

public class TestAuditDeleteHandler<T>(IStorageResolver storageResolver) 
  : TestModuleRequestHandler<TestAuditDeleteCommand<T>, Result>(storageResolver)
  where T : IConvertible

{
  public override async Task<Result> Handle(TestAuditDeleteCommand<T> request, CancellationToken cancellationToken)
  {
    var allTask = new List<Task>();
    foreach (var storage in WriteTestContexts())
    {
      switch (storage)
      {
        case TestModuleMongoStorageImpl:
          var t = storage.DeleteTestEntity<TestPKMongoEntity, ObjectId>((ObjectId)Convert.ChangeType(request.Id, typeof(ObjectId)));
          allTask.Add(t);
          break;
        default:
          var t2 = storage.DeleteTestEntity<TestAuditEntity, int>((int)Convert.ChangeType(request.Id, typeof(int)));
          allTask.Add(t2);
          break;
      }
    }
    await Task.WhenAll(allTask);
    return Result.Success();
  }
}