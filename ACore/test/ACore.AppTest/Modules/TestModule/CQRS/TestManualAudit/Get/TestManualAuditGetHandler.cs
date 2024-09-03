using ACore.AppTest.Modules.TestModule.CQRS.TestManualAudit.Models;
using ACore.AppTest.Modules.TestModule.Storages.SQL.Models;
using ACore.Models;
using ACore.Server.Storages;
using Microsoft.EntityFrameworkCore;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestManualAudit.Get;

internal class TestManualAuditGetHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestManualAuditGetQuery, TestManualAuditData[]>(storageResolver)
{
  public override async Task<Result<TestManualAuditData[]>> Handle(TestManualAuditGetQuery request, CancellationToken cancellationToken)
  {
    var db = ReadTestStorageWriteContexts().DbSet<TestManualAuditEntity>() ?? throw new Exception();
    var r = await db.Select(a => TestManualAuditData.Create(a)).ToArrayAsync(cancellationToken: cancellationToken);
    return Result.Success(r);
  }
}