using ACore.Base.CQRS.Models.Results;
using ACore.Server.Storages;
using ACore.Server.Storages.CQRS;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;

namespace ACore.Tests.TestImplementations.Server.Modules.TestModule.CQRS.TestNoAudit.Save;

internal class TestNoAuditSaveHandler(IStorageResolver storageResolver) : TestModuleRequestHandler<TestNoAuditSaveCommand, Result>(storageResolver)
{
  public override async Task<Result> Handle(TestNoAuditSaveCommand request, CancellationToken cancellationToken)
  {
    var allTask = new List<SaveHandlerData<TestNoAuditEntity>>();
    foreach (var storage in WriteTestContexts())
    {
      if (storage is TestModuleSqlStorageImpl)
      {
        var en = TestNoAuditEntity.Create(request.Data);
        allTask.Add(new SaveHandlerData<TestNoAuditEntity>(en, storage, storage.Save<TestNoAuditEntity, int>(en)));
      }
      else
        throw new Exception($"{nameof(TestNoAuditSaveHandler)} cannot be used for storage {storage.GetType().Name}");
    }

    await Task.WhenAll(allTask.Select(e => e.Task));
    return DbSaveResult.SuccessWithData(allTask);
  }
}