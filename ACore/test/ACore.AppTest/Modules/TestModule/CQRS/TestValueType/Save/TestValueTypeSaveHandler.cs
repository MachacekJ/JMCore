using ACore.AppTest.Modules.TestModule.Storages.SQL.Models;
using ACore.Extensions;
using ACore.Models;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestValueType.Save;

internal class TestValueTypeSaveHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestValueTypeSaveCommand>(storageResolver)
{
  public override async Task<Result> Handle(TestValueTypeSaveCommand request, CancellationToken cancellationToken)
  {
    var en = new TestValueTypeEntity();
    en.CopyPropertiesFrom(request.Data);
    //await WriteStorages().Save<TestValueTypeEntity, int>(en);
    return Result.Success();
  }
}