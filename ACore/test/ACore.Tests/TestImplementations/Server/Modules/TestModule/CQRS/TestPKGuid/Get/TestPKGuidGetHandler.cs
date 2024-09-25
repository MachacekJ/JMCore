using ACore.Base.CQRS.Models.Results;
using ACore.Server.Storages;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKGuid.Models;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using Microsoft.EntityFrameworkCore;


namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestPKGuid.Get;

internal class TestPKGuidGetHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestPKGuidGetQuery, Result<TestPKGuidData[]>>(storageResolver)
{
  public override async Task<Result<TestPKGuidData[]>> Handle(TestPKGuidGetQuery request, CancellationToken cancellationToken)
  {
    var db = ReadTestContext().DbSet<TestPKGuidEntity, Guid>() ?? throw new Exception();
    var r = await db.Select(a => TestPKGuidData.Create(a)).ToArrayAsync(cancellationToken: cancellationToken);
    return Result.Success(r);
  }
}