using ACore.AppTest.Modules.TestModule.Storages.EF.Models;
using ACore.Extensions;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.Test;

internal class TestSaveHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestSaveCommand, int>(storageResolver)
{
  public override async Task<int> Handle(TestSaveCommand request, CancellationToken cancellationToken)
  {
    var en = new TestEntity();
    en.CopyPropertiesFrom(request.Data);
    return await WriteStorage().Save<TestEntity, int>(en);
  }
}