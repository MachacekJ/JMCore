using ACore.Models;
using ACore.Server.Modules.AuditModule.UserProvider;
using ACore.Server.Storages;

namespace ACore.Server.Modules.AuditModule.CQRS.Audit.AuditSave;

internal class AuditSaveHandler(IStorageResolver storageResolver, IAuditUserProvider? userProvider) : AuditModuleRequestHandler<AuditSaveCommand, Result>(storageResolver)
{
  public override async Task<Result> Handle(AuditSaveCommand request, CancellationToken cancellationToken)
  {
    if (userProvider != null)
      request.AuditEntryItem.SetUser(userProvider.GetUser());
    
    List<Task> task =
    [
      ..AllBasicStorageWriteContexts().Select(context
        => context.SaveAuditAsync(request.AuditEntryItem))
    ];
    await Task.WhenAll(task);
    return Result.Success();
  }
}