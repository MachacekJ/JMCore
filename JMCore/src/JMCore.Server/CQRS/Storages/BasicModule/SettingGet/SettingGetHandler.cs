using JMCore.Server.Configuration.Storage;
using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Storages.Modules.BasicModule;

namespace JMCore.Server.CQRS.Storages.BasicModule.SettingGet;

public class SettingGetHandler(IStorageResolver storageResolver) : BasicDbRequestHandler<SettingGetQuery, string?>()
{
  public override Task<string?> Handle(SettingGetQuery request, CancellationToken cancellationToken)
  {
    var storageImplementation = storageResolver.FirstStorageModuleImplementation<IBasicStorageModule>(request.StorageType, StorageModeEnum.Read);
    return storageImplementation.Setting_GetAsync(request.Key, request.IsRequired);
  }
}