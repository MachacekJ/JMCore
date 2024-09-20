using ACore.Base.CQRS.Models;
using ACore.Base.CQRS.Models.Results;
using ACore.Server.Storages;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKLong.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using Microsoft.EntityFrameworkCore;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKLong.Get;

internal class TestPKLongAuditGetHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestPKLongAuditGetQuery, Result<TestPKLongAuditData[]>>(storageResolver)
{
  public override async Task<Result<TestPKLongAuditData[]>> Handle(TestPKLongAuditGetQuery request, CancellationToken cancellationToken)
  {
    var st = ReadTestContext();
    var db = st.DbSet<TestPKLongEntity>() ?? throw new Exception();
    var allItems = await db.ToArrayAsync(cancellationToken: cancellationToken);
    var rr = allItems.Select(TestPKLongAuditData.Create).ToArray();
    return Result.Success(rr);
  }
}