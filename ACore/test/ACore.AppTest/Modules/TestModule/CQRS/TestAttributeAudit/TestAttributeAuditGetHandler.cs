using ACore.AppTest.Modules.TestModule.Models;
using ACore.AppTest.Modules.TestModule.Storages.EF.Models;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit;

internal class TestAttributeAuditGetHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestAttributeAuditGetQuery, TestAttributeAuditData[]>(storageResolver)
{
  public override async Task<TestAttributeAuditData[]> Handle(TestAttributeAuditGetQuery request, CancellationToken cancellationToken)
  {
    return (await ReadTestStorageWriteContexts().All<TestAttributeAuditEntity>()).Select(TestAttributeAuditData.Create).ToArray();
  }
}