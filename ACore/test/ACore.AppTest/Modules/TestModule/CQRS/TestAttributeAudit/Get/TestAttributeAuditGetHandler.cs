using ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit.Models;
using ACore.AppTest.Modules.TestModule.Storages.Mongo;
using ACore.AppTest.Modules.TestModule.Storages.Mongo.Models;
using ACore.AppTest.Modules.TestModule.Storages.SQL.Models;
using ACore.Server.Storages;
using Microsoft.EntityFrameworkCore;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit.Get;

public class TestAttributeAuditGetHandler<T>(IStorageResolver storageResolver)
  : TestModuleRequestHandler<TestAttributeAuditGetQuery<T>, TestAttributeAuditData<T>[]>(storageResolver)
{
  public override async Task<TestAttributeAuditData<T>[]> Handle(TestAttributeAuditGetQuery<T> request, CancellationToken cancellationToken)
  {
    var st = ReadTestStorageWriteContexts();
    if (st is TestModuleMongoStorageImpl)
    {
      var dbMongo = st.DbSet<TestAttributeAuditPKMongoEntity>() ?? throw new Exception();
      var allItemsM = await dbMongo.ToArrayAsync(cancellationToken: cancellationToken);
      return allItemsM.Select(TestAttributeAuditData<T>.Create<T>).ToArray();
    }

    var db = st.DbSet<TestAttributeAuditEntity>() ?? throw new Exception();
    var allItems = await db.ToArrayAsync(cancellationToken: cancellationToken);
    return allItems.Select(TestAttributeAuditData<T>.Create<T>).ToArray();
  }
}