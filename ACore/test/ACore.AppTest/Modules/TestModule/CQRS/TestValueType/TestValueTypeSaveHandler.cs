using ACore.AppTest.Modules.TestModule.Storages.EF.Models;
using ACore.Extensions;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestValueType;

internal class TestValueTypeSaveHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestValueTypeSaveCommand, int>(storageResolver)
{
  public override async Task<int> Handle(TestValueTypeSaveCommand request, CancellationToken cancellationToken)
  {
    var en = new TestValueTypeEntity();
    en.CopyPropertiesFrom(request.Data);
    return await WriteStorage().Save<TestValueTypeEntity, int>(en);
  }
}