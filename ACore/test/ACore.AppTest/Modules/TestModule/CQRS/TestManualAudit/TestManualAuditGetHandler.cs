using ACore.AppTest.Modules.TestModule.Models;
using ACore.AppTest.Modules.TestModule.Storages.EF.Models;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestManualAudit;

internal class TestManualAuditGetHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestManualAuditGetQuery, IEnumerable<TestManualAuditData>>(storageResolver)
{
  public override async Task<IEnumerable<TestManualAuditData>> Handle(TestManualAuditGetQuery request, CancellationToken cancellationToken)
  {
    return (await ReadTestStorageWriteContexts().All<TestManualAuditEntity>()).Select(TestManualAuditData.Create);
  }
}