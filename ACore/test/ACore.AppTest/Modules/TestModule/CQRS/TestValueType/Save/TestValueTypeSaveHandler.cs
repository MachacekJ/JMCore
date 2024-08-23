using ACore.AppTest.Modules.TestModule.Storages.SQL.Models;
using ACore.Extensions;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestValueType.Save;

internal class TestValueTypeSaveHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestValueTypeSaveCommand, int>(storageResolver)
{
  public override async Task<int> Handle(TestValueTypeSaveCommand request, CancellationToken cancellationToken)
  {
    var en = new TestValueTypeEntity();
    en.CopyPropertiesFrom(request.Data);
    return await WriteStorage().Save<TestValueTypeEntity, int>(en);
  }
}