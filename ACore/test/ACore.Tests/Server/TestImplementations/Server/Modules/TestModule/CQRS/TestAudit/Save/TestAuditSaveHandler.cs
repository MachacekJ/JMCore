using ACore.Base.CQRS.Results;
using ACore.Server.Storages.CQRS;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.Mongo;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.Mongo.Models;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using MongoDB.Bson;
using TestAuditEntity = ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.Mongo.Models.TestAuditEntity;

namespace ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Save;

public class TestAuditSaveHandler<T>(IStorageResolver storageResolver)
  : TestModuleRequestHandler<TestAuditSaveCommand<T>, Result>(storageResolver)
  where T : IConvertible
{
  public override async Task<Result> Handle(TestAuditSaveCommand<T> request, CancellationToken cancellationToken)
  {
    var allTask = new List<SaveProcessExecutor>();

    foreach (var storage in WriteTestContexts())
    {
      switch (storage)
      {
        case TestModuleMongoStorageImpl:
          var enMongo = TestAuditEntity.Create(request.Data);
          allTask.Add(new SaveProcessExecutor(enMongo, storage, storage.SaveTestEntity<TestAuditEntity, ObjectId>(enMongo)));
          break;
        default:
          var en = Storages.SQL.Models.TestAuditEntity.Create(request.Data);
          allTask.Add(new SaveProcessExecutor(en, storage, storage.SaveTestEntity<Storages.SQL.Models.TestAuditEntity, int>(en)));
          break;
      }
    }

    await Task.WhenAll(allTask.Select(t => t.Task));
    return DbSaveResult.SuccessWithData(allTask);
  }
}