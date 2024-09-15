using ACore.Server.Storages;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestManualAudit.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using Microsoft.EntityFrameworkCore;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestManualAudit.Get;

internal class TestManualAuditGetHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestManualAuditGetQuery, TestManualAuditData[]>(storageResolver)
{
  public override async Task<Result<TestManualAuditData[]>> Handle(TestManualAuditGetQuery request, CancellationToken cancellationToken)
  {
    var db = ReadTestStorageWriteContexts().DbSet<TestManualAuditEntity>() ?? throw new Exception();
    var r = await db.Select(a => TestManualAuditData.Create(a)).ToArrayAsync(cancellationToken: cancellationToken);
    return Result.Success(r);
  }
}