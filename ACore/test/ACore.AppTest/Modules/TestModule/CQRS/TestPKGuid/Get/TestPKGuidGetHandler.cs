using ACore.AppTest.Modules.TestModule.CQRS.TestPKGuid.Models;
using ACore.AppTest.Modules.TestModule.Storages.SQL.Models;
using ACore.Models;
using ACore.Server.Storages;
using Microsoft.EntityFrameworkCore;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestPKGuid.Get;

internal class TestPKGuidGetHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestPKGuidGetQuery, TestPKGuidData[]>(storageResolver)
{
  public override async Task<Result<TestPKGuidData[]>> Handle(TestPKGuidGetQuery request, CancellationToken cancellationToken)
  {
    var db = ReadTestStorageWriteContexts().DbSet<TestPKGuidEntity>() ?? throw new Exception();
    var r = await db.Select(a => TestPKGuidData.Create(a)).ToArrayAsync(cancellationToken: cancellationToken);
    return Result.Success(r);
  }
}