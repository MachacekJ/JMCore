﻿using ACore.AppTest.Modules.TestModule.Models;
using ACore.AppTest.Modules.TestModule.Storages.EF.Models;
using ACore.Server.Storages;
using Microsoft.EntityFrameworkCore;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestValueType;

internal class TestValueTypeGetHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestValueTypeGetQuery, IEnumerable<TestValueTypeData>>(storageResolver)
{
  public override async Task<IEnumerable<TestValueTypeData>> Handle(TestValueTypeGetQuery request, CancellationToken cancellationToken)
  {
    var db = ReadTestStorageWriteContexts().DbSet<TestValueTypeEntity>() ?? throw new Exception();
    return await db.Select(a => TestValueTypeData.Create(a)).ToArrayAsync(cancellationToken: cancellationToken);
  }
}