using ACore.AppTest.Modules.TestModule.Models;
using ACore.AppTest.Modules.TestModule.Storages.EF.Models;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestPKString;

internal class TestPKStringSaveHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestPKStringSaveCommand, string>(storageResolver)
{
  public override async Task<string> Handle(TestPKStringSaveCommand request, CancellationToken cancellationToken)
  {
    return await WriteStorage().Save<TestPKStringEntity, string>(request.Data.ToEntity());
  }
}