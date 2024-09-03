using ACore.AppTest.Modules.TestModule.Storages.SQL.Models;
using ACore.Models;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestPKGuid.Save;

internal class TestPKGuidSaveHandler(IStorageResolver storageResolver) 
  : TestModuleRequestHandler<TestPKGuidSaveCommand>(storageResolver)
{
  public override async Task<Result> Handle(TestPKGuidSaveCommand request, CancellationToken cancellationToken)
  {
    //await WriteStorages().Save<TestPKGuidEntity, Guid>(request.Data.ToEntity());
    return Result.Success();
  }
}