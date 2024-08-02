using ACore.AppTest.Modules.TestModule.CQRS.Models;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestValueType;

internal class TestValueTypeGetHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestValueTypeGetQuery, IEnumerable<TestValueTypeData>>(storageResolver)
{
  public override async Task<IEnumerable<TestValueTypeData>> Handle(TestValueTypeGetQuery request, CancellationToken cancellationToken)
  {
    return (await ReadTestStorageWriteContexts().AllTestValueTypeString()).Select(TestValueTypeData.Create);
  }
}