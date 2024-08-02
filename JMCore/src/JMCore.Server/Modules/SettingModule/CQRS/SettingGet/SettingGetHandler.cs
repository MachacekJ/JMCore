using JMCore.Server.Modules.SettingModule.Storage;
using JMCore.Server.Storages;
using JMCore.Server.Storages.Models;

namespace JMCore.Server.Modules.SettingModule.CQRS.SettingGet;

public class SettingGetHandler(IStorageResolver storageResolver) : SettingModuleRequestHandler<SettingGetQuery, string?>()
{
  public override Task<string?> Handle(SettingGetQuery request, CancellationToken cancellationToken)
  {
    var storageImplementation = storageResolver.FirstReadWriteStorage<IBasicStorageModule>(request.StorageType, StorageModeEnum.Read);
    return storageImplementation.Setting_GetAsync(request.Key, request.IsRequired);
  }
}