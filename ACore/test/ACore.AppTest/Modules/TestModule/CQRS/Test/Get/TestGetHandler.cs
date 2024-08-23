using ACore.AppTest.Modules.TestModule.CQRS.Test.Models;
using ACore.AppTest.Modules.TestModule.Storages.SQL.Models;
using ACore.Server.Storages;
using Microsoft.EntityFrameworkCore;

namespace ACore.AppTest.Modules.TestModule.CQRS.Test.Get;

internal class TestGetHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestGetQuery, IEnumerable<TestData>>(storageResolver)
{
  public override async Task<IEnumerable<TestData>> Handle(TestGetQuery request, CancellationToken cancellationToken)
  {
    var db = ReadTestStorageWriteContexts().DbSet<TestEntity>() ?? throw new Exception();
    return await db.Select(a => TestData.Create(a)).ToArrayAsync(cancellationToken: cancellationToken);
  }
}