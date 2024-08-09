using ACore.AppTest.Modules.TestModule.Models;
using ACore.AppTest.Modules.TestModule.Storages.EF.Models;
using ACore.Server.Storages;
using Microsoft.EntityFrameworkCore;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestPKString;

internal class TestPKStringGetHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestPKStringGetQuery, IEnumerable<TestPKStringData>>(storageResolver)
{
  public override async Task<IEnumerable<TestPKStringData>> Handle(TestPKStringGetQuery request, CancellationToken cancellationToken)
  {
    var db = ReadTestStorageWriteContexts().DbSet<TestPKStringEntity>() ?? throw new Exception();
    return await db.Select(a => TestPKStringData.Create(a)).ToArrayAsync(cancellationToken: cancellationToken);
  }
}