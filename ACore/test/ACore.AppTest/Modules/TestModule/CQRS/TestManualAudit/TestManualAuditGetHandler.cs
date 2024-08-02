using ACore.AppTest.Modules.TestModule.CQRS.Models;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestManualAudit;

internal class TestManualAuditGetHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestManualAuditGetQuery, IEnumerable<TestManualAuditData>>(storageResolver)
{
  public override async Task<IEnumerable<TestManualAuditData>> Handle(TestManualAuditGetQuery request, CancellationToken cancellationToken)
  {
    return (await ReadTestStorageWriteContexts().AllTestManual()).Select(TestManualAuditData.Create);
  }
}