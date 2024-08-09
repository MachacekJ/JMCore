using ACore.Base.CQRS.Results;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestPKString.Models;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using Microsoft.EntityFrameworkCore;

namespace ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestPKString.Get;

internal class TestPKStringGetHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestPKStringGetQuery, Result<TestPKStringData[]>>(storageResolver)
{
  public override async Task<Result<TestPKStringData[]>> Handle(TestPKStringGetQuery request, CancellationToken cancellationToken)
  {
    var db = ReadTestContext().DbSet<TestPKStringEntity, string>() ?? throw new Exception();
    var r = await db.Select(a => TestPKStringData.Create(a)).ToArrayAsync(cancellationToken: cancellationToken);
    return Result.Success(r);
  }
}