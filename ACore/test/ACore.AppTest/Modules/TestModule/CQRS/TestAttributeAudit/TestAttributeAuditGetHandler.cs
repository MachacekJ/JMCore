using ACore.AppTest.Modules.TestModule.Models;
using ACore.AppTest.Modules.TestModule.Storages.EF.Models;
using ACore.Server.Storages;
using Microsoft.EntityFrameworkCore;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit;

internal class TestAttributeAuditGetHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestAttributeAuditGetQuery, TestAttributeAuditData[]>(storageResolver)
{
  public override async Task<TestAttributeAuditData[]> Handle(TestAttributeAuditGetQuery request, CancellationToken cancellationToken)
  {
    var db = ReadTestStorageWriteContexts().DbSet<TestAttributeAuditEntity>() ?? throw new Exception();
    return await db.Select(a => TestAttributeAuditData.Create(a)).ToArrayAsync(cancellationToken: cancellationToken);
  }
}