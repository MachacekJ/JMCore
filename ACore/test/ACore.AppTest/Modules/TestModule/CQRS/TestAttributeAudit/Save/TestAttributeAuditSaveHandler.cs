using ACore.AppTest.Modules.TestModule.Storages.Mongo;
using ACore.AppTest.Modules.TestModule.Storages.Mongo.Models;
using ACore.AppTest.Modules.TestModule.Storages.SQL.Models;
using ACore.Extensions;
using ACore.Models;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit.Save;

public class TestAttributeAuditSaveHandler<T>(IStorageResolver storageResolver) 
  : TestModuleRequestHandler<TestAttributeAuditSaveCommand<T>>(storageResolver)
  where T: IConvertible
{
  public override async Task<Result> Handle(TestAttributeAuditSaveCommand<T> request, CancellationToken cancellationToken)
  {
    var st = WriteStorages().First();
    if (st is TestModuleMongoStorageImpl)
    {
      var enMongo = new TestAttributeAuditPKMongoEntity();
      enMongo.CopyPropertiesFrom(request.Data);
     // await WriteStorages().Save<TestAttributeAuditPKMongoEntity, T>(enMongo);
      request.Data.CopyPropertiesFrom(enMongo);
    }
    
    var en = new TestAttributeAuditEntity();
    en.CopyPropertiesFrom(request.Data);
  //  await WriteStorages().Save<TestAttributeAuditEntity, T>(en);
    request.Data.CopyPropertiesFrom(en);
    return Result.Success();
  }
}