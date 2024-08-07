using ACore.AppTest.Modules.TestModule.Models;
using ACore.AppTest.Modules.TestModule.Storages.EF.Models;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestValueType;

internal class TestValueTypeGetHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestValueTypeGetQuery, IEnumerable<TestValueTypeData>>(storageResolver)
{
  public override async Task<IEnumerable<TestValueTypeData>> Handle(TestValueTypeGetQuery request, CancellationToken cancellationToken)
  {
    return (await ReadTestStorageWriteContexts().All<TestValueTypeEntity>()).Select(TestValueTypeData.Create);
  }
}