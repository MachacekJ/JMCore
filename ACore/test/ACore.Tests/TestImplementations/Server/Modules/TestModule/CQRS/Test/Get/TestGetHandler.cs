using ACore.Base.CQRS.Models;
using ACore.Server.Storages;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.Test.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using Microsoft.EntityFrameworkCore;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.Test.Get;

internal class TestGetHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestGetQuery, Result<TestData[]>>(storageResolver)
{
  public override async Task<Result<TestData[]>> Handle(TestGetQuery request, CancellationToken cancellationToken)
  {
    var db = ReadTestContext().DbSet<TestEntity>() ?? throw new Exception();
    var testData = await db.Select(a => TestData.Create(a)).ToArrayAsync(cancellationToken: cancellationToken);
    return Result.Success(testData);
  }
}