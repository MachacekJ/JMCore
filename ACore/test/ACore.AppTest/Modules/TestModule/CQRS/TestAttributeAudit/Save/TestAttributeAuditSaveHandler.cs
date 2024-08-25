﻿using ACore.AppTest.Modules.TestModule.Storages.Mongo;
using ACore.AppTest.Modules.TestModule.Storages.Mongo.Models;
using ACore.AppTest.Modules.TestModule.Storages.SQL.Models;
using ACore.Extensions;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit.Save;

public class TestAttributeAuditSaveHandler<T>(IStorageResolver storageResolver) 
  : TestModuleRequestHandler<TestAttributeAuditSaveCommand<T>, T>(storageResolver)
  where T: IConvertible
{
  public override async Task<T> Handle(TestAttributeAuditSaveCommand<T> request, CancellationToken cancellationToken)
  {
    var st = WriteStorage();
    if (st is TestModuleMongoStorageImpl)
    {
      var enMongo = new TestAttributeAuditPKMongoEntity();
      enMongo.CopyPropertiesFrom(request.Data);
      return await WriteStorage().Save<TestAttributeAuditPKMongoEntity, T>(enMongo);
    }
    
    var en = new TestAttributeAuditEntity();
    en.CopyPropertiesFrom(request.Data);
    return await WriteStorage().Save<TestAttributeAuditEntity, T>(en);
  }
}