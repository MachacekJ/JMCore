using ACore.AppTest.Modules.TestModule.CQRS.TestPKString.Models;
using ACore.AppTest.Modules.TestModule.Storages.SQL.Models;
using ACore.Models;
using ACore.Server.Storages;
using Microsoft.EntityFrameworkCore;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestPKString.Get;

internal class TestPKStringGetHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestPKStringGetQuery, TestPKStringData[]>(storageResolver)
{
  public override async Task<Result<TestPKStringData[]>> Handle(TestPKStringGetQuery request, CancellationToken cancellationToken)
  {
    var db = ReadTestStorageWriteContexts().DbSet<TestPKPKStringEntity>() ?? throw new Exception();
    var r = await db.Select(a => TestPKStringData.Create(a)).ToArrayAsync(cancellationToken: cancellationToken);
    return Result.Success(r);
  }
}