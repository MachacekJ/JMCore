using ACore.AppTest.Modules.TestModule.CQRS.Models;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit;

internal class TestAttributeAuditGetHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestAttributeAuditGetQuery, IEnumerable<TestAttributeAuditData>>(storageResolver)
{
  public override async Task<IEnumerable<TestAttributeAuditData>> Handle(TestAttributeAuditGetQuery request, CancellationToken cancellationToken)
  {
    return (await ReadTestStorageWriteContexts().AllTestAttribute()).Select(TestAttributeAuditData.Create);
  }
}