using System.Runtime.ExceptionServices;
using ACore.Base.CQRS.Results;
using ACore.Server.Configuration;
using ACore.Server.Storages.CQRS;
using ACore.Server.Storages.Services.StorageResolvers;
using Microsoft.Extensions.Options;

namespace ACore.Server.Modules.AuditModule.CQRS.AuditSave;

public class AuditSaveHandler(IStorageResolver storageResolver, IOptions<ACoreServerOptions> serverOptions) : AuditModuleRequestHandler<AuditSaveCommand, Result>(storageResolver)
{
  public override async Task<Result> Handle(AuditSaveCommand request, CancellationToken cancellationToken)
  {
    var allTask = new List<SaveProcessExecutor>();
    foreach (var storage in WriteAuditContexts())
    {
      allTask.Add(new SaveProcessExecutor(request.SaveInfoItem, storage, storage.SaveAuditAsync(request.SaveInfoItem)));
    }

    Task? taskSum = null;
    try
    {
      taskSum = Task.WhenAll(allTask.Select(e => e.Task));
      await taskSum.ConfigureAwait(false);
    }
    catch
    {
      if (taskSum?.Exception != null) ExceptionDispatchInfo.Capture(taskSum.Exception).Throw();
      throw;
    }

    return DbSaveResult.SuccessWithData(allTask, serverOptions.Value.ACoreOptions.SaltForHash);
  }
}