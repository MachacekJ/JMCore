using ACore.Base.CQRS.Models;
using ACore.Server.Configuration;
using ACore.Server.Storages;
using ACore.Server.Storages.CQRS;
using ACore.Server.Storages.Models;
using Microsoft.Extensions.Options;

namespace ACore.Server.Modules.AuditModule.CQRS.Audit.AuditSave;

internal class AuditSaveHandler(IStorageResolver storageResolver, IOptions<ACoreServerOptions> serverOptions) : AuditModuleRequestHandler<AuditSaveCommand, Result>(storageResolver)
{
  public override async Task<Result> Handle(AuditSaveCommand request, CancellationToken cancellationToken)
  {
    var userProvider = serverOptions.Value.AuditModuleOptions.AuditUserProvider;
    if (userProvider != null)
      request.AuditEntryItem.SetUser(userProvider.GetUser());

    var allTask = new List<SaveHandlerData>();
    foreach (var storage in WriteAuditContexts())
    {
      allTask.Add(new SaveHandlerData(request.AuditEntryItem, storage, storage.SaveAuditAsync(request.AuditEntryItem)));
    }

    await Task.WhenAll(allTask.Select(t => t.Task));

    var pkValues = allTask.ToDictionary<SaveHandlerData?, StorageTypeEnum, object>(
      saveHandlerData => saveHandlerData.Storage.StorageDefinition.Type, 
      _ => Result.Success());
    
    return DbSaveResult.SuccessWithPkValues(pkValues);
  }
}