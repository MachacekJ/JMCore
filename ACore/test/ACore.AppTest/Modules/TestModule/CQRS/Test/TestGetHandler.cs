using ACore.AppTest.Modules.TestModule.CQRS.TestPKGuid;
using ACore.AppTest.Modules.TestModule.Models;
using ACore.AppTest.Modules.TestModule.Storages.EF.Models;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.Test;

internal class TestGetHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestGetQuery, IEnumerable<TestData>>(storageResolver)
{
  public override async Task<IEnumerable<TestData>> Handle(TestGetQuery request, CancellationToken cancellationToken)
  {
    return (await ReadTestStorageWriteContexts().All<TestEntity>()).Select(TestData.Create);
  }
}