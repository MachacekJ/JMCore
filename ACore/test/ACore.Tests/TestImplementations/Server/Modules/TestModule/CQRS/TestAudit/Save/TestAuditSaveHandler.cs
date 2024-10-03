using ACore.Base.CQRS.Models.Results;
using ACore.Server.Storages;
using ACore.Server.Storages.CQRS;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.Mongo;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.Mongo.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using MongoDB.Bson;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Save;

public class TestAuditSaveHandler<T>(IStorageResolver storageResolver)
  : TestModuleRequestHandler<TestAuditSaveCommand<T>, Result>(storageResolver)
  where T : IConvertible
{
  public override async Task<Result> Handle(TestAuditSaveCommand<T> request, CancellationToken cancellationToken)
  {
    var allTask = new List<SavingProcessData>();

    foreach (var storage in WriteTestContexts())
    {
      switch (storage)
      {
        case TestModuleMongoStorageImpl:
          var enMongo = TestPKMongoEntity.Create(request.Data);
          allTask.Add(new SavingProcessData(enMongo, storage, storage.SaveTestEntity<TestPKMongoEntity, ObjectId>(enMongo)));
          break;
        default:
          var en = TestAuditEntity.Create(request.Data);
          allTask.Add(new SavingProcessData(en, storage, storage.SaveTestEntity<TestAuditEntity, int>(en)));
          break;
      }
    }

    await Task.WhenAll(allTask.Select(t => t.Task));
    return DbSaveResult.SuccessWithData(allTask);
  }
}