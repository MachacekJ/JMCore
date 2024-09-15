using ACore.Base.CQRS.Models;
using ACore.Server.Storages;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAttributeAudit.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.Mongo;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.Mongo.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using Microsoft.EntityFrameworkCore;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestAttributeAudit.Get;

public class TestAttributeAuditGetHandler<T>(IStorageResolver storageResolver)
  : TestModuleRequestHandler<TestAttributeAuditGetQuery<T>,Result<TestAttributeAuditData<T>[]>>(storageResolver)
{
  public override async Task<Result<TestAttributeAuditData<T>[]>> Handle(TestAttributeAuditGetQuery<T> request, CancellationToken cancellationToken)
  {
    var st = ReadTestContext();
    if (st is TestModuleMongoStorageImpl)
    {
      var dbMongo = st.DbSet<TestAttributeAuditPKMongoEntity>() ?? throw new Exception();
      var allItemsM = await dbMongo.ToArrayAsync(cancellationToken: cancellationToken);
      var r = allItemsM.Select(TestAttributeAuditData<T>.Create<T>).ToArray();
      return Result.Success(r);
    }

    var db = st.DbSet<TestAttributeAuditPKIntEntity>() ?? throw new Exception();
    var allItems = await db.ToArrayAsync(cancellationToken: cancellationToken);
    var rr = allItems.Select(TestAttributeAuditData<T>.Create<T>).ToArray();
    return Result.Success(rr);
  }
}