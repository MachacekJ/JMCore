using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Storages.Modules.BasicModule;

namespace JMCore.TestsIntegrations.ServerT.StoragesT.BasicStructureT;

public class BasicStructureBaseT : StorageBaseT
{
  protected const StorageTypeEnum StorageTypesToTest = StorageTypeEnum.Postgres | StorageTypeEnum.Memory;

  protected IBasicStorageModule GetBasicStorageModule(StorageTypeEnum storageType) => StorageResolver.FirstStorageModuleImplementation<IBasicStorageModule>(storageType);
  
}