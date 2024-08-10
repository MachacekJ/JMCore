﻿using ACore.AppTest.Modules.TestModule.Storages.EF.Models;
using ACore.AppTest.Modules.TestModule.Storages.Mongo;
using ACore.AppTest.Modules.TestModule.Storages.Mongo.Models;
using ACore.Extensions;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit;

public class TestAttributeAuditSaveHandler<T>(IStorageResolver storageResolver) : TestModuleRequestHandler<TestAttributeAuditSaveCommand<T>, T>(storageResolver)
{
  public override async Task<T> Handle(TestAttributeAuditSaveCommand<T> request, CancellationToken cancellationToken)
  {
    var st = WriteStorage();
    if (st is EfTestMongoStorageImpl)
    {
      var enMongo = new TestAttributeAuditMongoEntity();
      enMongo.CopyPropertiesFrom(request.Data);
      return await WriteStorage().Save<TestAttributeAuditMongoEntity, T>(enMongo);
    }


    var en = new TestAttributeAuditEntity();
    en.CopyPropertiesFrom(request.Data);
    return await WriteStorage().Save<TestAttributeAuditEntity, T>(en);
  }
}