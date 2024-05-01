using JMCore.Server.Configuration.Storage;

namespace JMCore.Server.CQRS.DB.BasicStructure.SettingSave;

public class SettingSaveHandler(IStorageResolver storageResolver) : BasicDbRequestHandler<SettingSaveCommand>(storageResolver)
{
  public override async Task Handle(SettingSaveCommand request, CancellationToken cancellationToken)
  {
    List<Task> task = [..BasicDbWriteContexts(request.StorageType).Select(context => context.Setting_SaveAsync(request.Key, request.Value, request.IsSystem))];
    await Task.WhenAll(task);
  }
}