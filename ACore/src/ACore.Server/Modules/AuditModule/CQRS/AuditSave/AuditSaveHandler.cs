using ACore.Base.CQRS.Results;
using ACore.Server.Configuration;
using ACore.Server.Storages.CQRS;
using ACore.Server.Storages.Services.StorageResolvers;
using Microsoft.Extensions.Options;

namespace ACore.Server.Modules.AuditModule.CQRS.AuditSave;

internal class AuditSaveHandler(IStorageResolver storageResolver, IOptions<ACoreServerOptions> serverOptions) : AuditModuleRequestHandler<AuditSaveCommand, Result>(storageResolver)
{
  public override async Task<Result> Handle(AuditSaveCommand request, CancellationToken cancellationToken)
  {
    var allTask = new List<SavingProcessData>();
    foreach (var storage in WriteAuditContexts())
    {
      allTask.Add(new SavingProcessData(request.AuditEntryItem, storage, storage.SaveAuditAsync(request.AuditEntryItem)));
    }

    await Task.WhenAll(allTask.Select(t => t.Task));
    return DbSaveResult.SuccessWithData(allTask, serverOptions.Value.ACoreOptions.SaltForHash);
  }
}