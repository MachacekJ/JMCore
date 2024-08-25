using ACore.AppTest.Modules.TestModule.Storages.SQL.Models;
using ACore.Server.Storages;

namespace ACore.AppTest.Modules.TestModule.CQRS.TestPKGuid.Save;

internal class TestPKGuidSaveHandler(IStorageResolver storageResolver) 
  : TestModuleRequestHandler<TestPKGuidSaveCommand, Guid>(storageResolver)
{
  public override async Task<Guid> Handle(TestPKGuidSaveCommand request, CancellationToken cancellationToken)
  {
    return await WriteStorage().Save<TestPKGuidEntity, Guid>(request.Data.ToEntity());
  }
}