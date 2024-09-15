using ACore.Base.CQRS.Models;
using ACore.Extensions;
using ACore.Server.Storages;
using ACore.Server.Storages.CQRS;
using ACore.Server.Storages.Models.PK;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.Mongo;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.Mongo.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAttributeAudit.Save;

public class TestAttributeAuditSaveHandler<T>(IStorageResolver storageResolver)
  : TestModuleRequestHandler<TestAttributeAuditSaveCommand<T>, Result>(storageResolver)
  where T : IConvertible
{
  public override async Task<Result> Handle(TestAttributeAuditSaveCommand<T> request, CancellationToken cancellationToken)
  {
    var allTask = new List<SaveHandlerData>();
    
    foreach (var storage in WriteTestContexts())
    {
      switch (storage)
      {
        case TestModuleMongoStorageImpl:
          var enMongo = TestAttributeAuditPKMongoEntity.Create(request.Data);
          allTask.Add(new SaveHandlerData(enMongo, storage, storage.Save<TestAttributeAuditPKMongoEntity, T>(enMongo)));
          break;
        default:
          var en = TestAttributeAuditPKIntEntity.Create(request.Data);
          allTask.Add(new SaveHandlerData(en, storage, storage.Save<TestAttributeAuditPKIntEntity, T>(en)));
          break;
      }
    }

    await Task.WhenAll(allTask.Select(t=>t.Task));
    return DbSaveResult.SuccessWithPkValues(allTask.ToDictionary(
      k => k.Storage.StorageDefinition.Type, 
      v => v.Entity.PropertyValue(nameof(PKEntity<int>.Id)) 
           ?? throw new Exception($"{nameof(PKEntity<int>.Id)} is null.")));
  }
}