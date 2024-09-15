using ACore.Base.CQRS.Models;
using ACore.Server.Storages;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKString.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using Microsoft.EntityFrameworkCore;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKString.Get;

internal class TestPKStringGetHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestPKStringGetQuery, Result<TestPKStringData[]>>(storageResolver)
{
  public override async Task<Result<TestPKStringData[]>> Handle(TestPKStringGetQuery request, CancellationToken cancellationToken)
  {
    var db = ReadTestContext().DbSet<TestPKStringEntity>() ?? throw new Exception();
    var r = await db.Select(a => TestPKStringData.Create(a)).ToArrayAsync(cancellationToken: cancellationToken);
    return Result.Success(r);
  }
}