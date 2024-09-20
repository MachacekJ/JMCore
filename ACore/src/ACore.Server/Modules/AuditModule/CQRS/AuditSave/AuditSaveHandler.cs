using ACore.Base.CQRS.Models;
using ACore.Base.CQRS.Models.Results;
using ACore.Server.Configuration;
using ACore.Server.Storages;
using ACore.Server.Storages.CQRS;
using ACore.Server.Storages.Models;
using Microsoft.Extensions.Options;

namespace ACore.Server.Modules.AuditModule.CQRS.AuditSave;

internal class AuditSaveHandler(IStorageResolver storageResolver, IOptions<ACoreServerOptions> serverOptions) : AuditModuleRequestHandler<AuditSaveCommand, Result>(storageResolver)
{
  public override async Task<Result> Handle(AuditSaveCommand request, CancellationToken cancellationToken)
  {
    // var userProvider = serverOptions.Value.AuditModuleOptions.AuditUserProvider;
    // if (userProvider != null)
    //   request.AuditEntryItem.SetUser(userProvider.GetUser());

    var allTask = new List<SaveHandlerData>();
    foreach (var storage in WriteAuditContexts())
    {
      allTask.Add(new SaveHandlerData(request.AuditEntryItem, storage, storage.SaveAuditAsync(request.AuditEntryItem)));
    }

    await Task.WhenAll(allTask.Select(t => t.Task));
    return DbSaveResult.SuccessWithData(allTask, serverOptions.Value.ACoreOptions.SaltForHash);
  }
}