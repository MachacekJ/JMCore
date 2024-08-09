using ACore.Base.CQRS.Results;
using ACore.Server.Configuration.CQRS.OptionsGet;
using ACore.Server.Storages.CQRS;
using ACore.Server.Storages.Services.StorageResolvers;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.SQL;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.SQL.Models;
using MediatR;

namespace ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestNoAudit.Save;

internal class TestNoAuditSaveHandler(IStorageResolver storageResolver, IMediator mediator) : TestModuleRequestHandler<TestNoAuditSaveCommand, Result>(storageResolver)
{
  public override async Task<Result> Handle(TestNoAuditSaveCommand request, CancellationToken cancellationToken)
  {
    var saltForHash = (await mediator.Send(new AppOptionQuery<string>(OptionQueryEnum.HashSalt), cancellationToken)).ResultValue 
                      ?? throw new Exception($"Mediator for {nameof(AppOptionQuery<string>)}.{Enum.GetName(OptionQueryEnum.HashSalt)} returned null value.");

    var allTask = new List<SaveProcessExecutor<TestNoAuditEntity>>();
    foreach (var storage in WriteTestContexts())
    {
      if (storage is TestModuleSqlStorageImpl)
      {
        var en = TestNoAuditEntity.Create(request.Data);
        allTask.Add(new SaveProcessExecutor<TestNoAuditEntity>(en, storage, storage.SaveTestEntity<TestNoAuditEntity, int>(en, request.Hash)));
      }
      else
        throw new Exception($"{nameof(TestNoAuditSaveHandler)} cannot be used for storage {storage.GetType().Name}");
    }

    await Task.WhenAll(allTask.Select(e => e.Task));
    return DbSaveResult.SuccessWithData(allTask, saltForHash);
  }
}