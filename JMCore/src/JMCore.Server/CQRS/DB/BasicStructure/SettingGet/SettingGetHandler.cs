using JMCore.Server.Configuration.Storage;
using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Storages.Modules.BasicModule;

namespace JMCore.Server.CQRS.DB.BasicStructure.SettingGet;

public class SettingGetHandler(IStorageResolver storageResolver) : BasicDbRequestHandler<SettingGetQuery, string?>(storageResolver)
{
  private readonly IStorageResolver _storageResolver = storageResolver;

  public override Task<string?> Handle(SettingGetQuery request, CancellationToken cancellationToken)
  {
    var storageImplementation = _storageResolver.StorageModuleImplementation<IBasicStorageModule>(request.StorageType, StorageModeEnum.Read);
    return storageImplementation.Setting_GetAsync(request.Key, request.IsRequired);
  }
}