using ACore.AppTest.Modules.TestModule.CQRS.TestPKString.Models;
using ACore.AppTest.Modules.TestModule.Storages.SQL.Models;
using ACore.Models;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestPKString.Save;

internal class TestPKStringSaveHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestPKStringSaveCommand>(storageResolver)
{
  public override async Task<Result> Handle(TestPKStringSaveCommand request, CancellationToken cancellationToken)
  {
   //1await WriteStorages().Save<TestPKPKStringEntity, string>(request.Data.ToEntity());
   return Result.Success();
  }
}