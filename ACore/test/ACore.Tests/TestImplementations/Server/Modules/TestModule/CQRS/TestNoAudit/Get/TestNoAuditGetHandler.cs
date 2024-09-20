using ACore.Base.CQRS.Models;
using ACore.Base.CQRS.Models.Results;
using ACore.Server.Storages;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestNoAudit.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using Microsoft.EntityFrameworkCore;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestNoAudit.Get;

internal class TestNoAuditGetHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestNoAuditGetQuery, Result<TestNoAuditData[]>>(storageResolver)
{
  public override async Task<Result<TestNoAuditData[]>> Handle(TestNoAuditGetQuery request, CancellationToken cancellationToken)
  {
    var db = ReadTestContext().DbSet<TestNoAuditEntity>() ?? throw new Exception();
    var testData = await db.Select(a => TestNoAuditData.Create(a)).ToArrayAsync(cancellationToken: cancellationToken);
    return Result.Success(testData);
  }
}