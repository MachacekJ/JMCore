using ACore.AppTest.Modules.TestModule.CQRS.TestValueType.Models;
using ACore.AppTest.Modules.TestModule.Storages.SQL.Models;
using ACore.Models;
using ACore.Server.Storages;
using Microsoft.EntityFrameworkCore;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestValueType.Get;

internal class TestValueTypeGetHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestValueTypeGetQuery, TestValueTypeData[]>(storageResolver)
{
  public override async Task<Result<TestValueTypeData[]>> Handle(TestValueTypeGetQuery request, CancellationToken cancellationToken)
  {
    var db = ReadTestStorageWriteContexts().DbSet<TestValueTypeEntity>() ?? throw new Exception();
    var r= await db.Select(a => TestValueTypeData.Create(a)).ToArrayAsync(cancellationToken: cancellationToken);
    return Result.Success(r);
  }
}