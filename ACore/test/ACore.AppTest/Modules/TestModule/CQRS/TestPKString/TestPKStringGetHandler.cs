using ACore.AppTest.Modules.TestModule.Models;
using ACore.AppTest.Modules.TestModule.Storages.EF.Models;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestPKString;

internal class TestPKStringGetHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestPKStringGetQuery, IEnumerable<TestPKStringData>>(storageResolver)
{
  public override async Task<IEnumerable<TestPKStringData>> Handle(TestPKStringGetQuery request, CancellationToken cancellationToken)
  {
    return (await ReadTestStorageWriteContexts().All<TestPKStringEntity>()).Select(TestPKStringData.Create);
  }
}