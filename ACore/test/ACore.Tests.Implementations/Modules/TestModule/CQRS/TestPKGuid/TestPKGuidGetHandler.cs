using ACore.Server.Storages;
using ACore.Tests.Implementations.Modules.TestModule.CQRS.Models;

namespace ACore.Tests.Implementations.Modules.TestModule.CQRS.TestPKGuid;

public class TestPKGuidGetHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestPKGuidGetQuery, IEnumerable<TestPKGuidData>>(storageResolver)
{
  public override async Task<IEnumerable<TestPKGuidData>> Handle(TestPKGuidGetQuery request, CancellationToken cancellationToken)
  {
    return (await ReadTestStorageWriteContexts().AllTestPKGuid()).Select(TestPKGuidData.Create);
  }
}