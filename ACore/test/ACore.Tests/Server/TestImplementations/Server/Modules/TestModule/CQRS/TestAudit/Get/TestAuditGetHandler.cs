using ACore.Base.CQRS.Results;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Models;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.Mongo;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.Mongo.Models;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestAudit.Get;

public class TestAuditGetHandler<T>(IStorageResolver storageResolver)
  : TestModuleRequestHandler<TestAuditGetQuery<T>,Result<TestAuditData<T>[]>>(storageResolver)
{
  public override async Task<Result<TestAuditData<T>[]>> Handle(TestAuditGetQuery<T> request, CancellationToken cancellationToken)
  {
    var st = ReadTestContext();
    if (st is TestModuleMongoStorageImpl)
    {
      var dbMongo = st.DbSet<TestPKMongoEntity, ObjectId>() ?? throw new Exception();
      var allItemsM = await dbMongo.ToArrayAsync(cancellationToken: cancellationToken);
      var r = allItemsM.Select(TestAuditData<T>.Create<T>).ToArray();
      return Result.Success(r);
    }

    var db = st.DbSet<TestAuditEntity, int>() ?? throw new Exception();
    var allItems = await db.ToArrayAsync(cancellationToken: cancellationToken);
    var rr = allItems.Select(TestAuditData<T>.Create<T>).ToArray();
    return Result.Success(rr);
  }
}