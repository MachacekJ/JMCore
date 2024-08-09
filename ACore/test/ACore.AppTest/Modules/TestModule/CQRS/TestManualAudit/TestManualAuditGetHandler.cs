﻿using ACore.AppTest.Modules.TestModule.Models;
using ACore.AppTest.Modules.TestModule.Storages.EF.Models;
using ACore.Server.Storages;
using Microsoft.EntityFrameworkCore;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestManualAudit;

internal class TestManualAuditGetHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestManualAuditGetQuery, TestManualAuditData[]>(storageResolver)
{
  public override async Task<TestManualAuditData[]> Handle(TestManualAuditGetQuery request, CancellationToken cancellationToken)
  {
    var db = ReadTestStorageWriteContexts().DbSet<TestManualAuditEntity>() ?? throw new Exception();
    return await db.Select(a => TestManualAuditData.Create(a)).ToArrayAsync(cancellationToken: cancellationToken);
  }
}