using System.Diagnostics;
using System.Reflection;
using ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit;
using ACore.AppTest.Modules.TestModule.Models;
using ACore.Server.Storages.Models;
using MongoDB.Bson;
using Xunit;

namespace ACore.TestsStress;

public class AuditMongo : AuditBase
{
  //protected override StorageTypeEnum StorageTypesToTest => StorageTypeEnum.Mongo;

  [Fact]
  public async Task UpdateItem()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypeEnum.Mongo, method, async (storageType) =>
    {
      for (var i = 0; i < 10000; i++)
      {
        var item = new TestAttributeAuditData<ObjectId>
        {
          Created = DateTime.UtcNow,
          Name = "fdfsdfsdafsaasdfsdafa" + i,
          NotAuditableColumn = "Audit"
        };
        var res = await Mediator.Send(new TestAttributeAuditSaveCommand<ObjectId>(item));
        if (i % 10 == 0)
          Debug.WriteLine(i);
      }
    });
  }
}