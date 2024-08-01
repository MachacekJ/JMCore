using JMCore.Server.Modules.SettingModule.Storage;
using JMCore.Server.Storages.Models;

namespace JMCore.TestsIntegrations.ServerT.StoragesT.ModulesT.BasicStorageT;

public class BasicStructureBaseT : StorageBaseT
{
  protected static StorageTypeEnum StorageTypesToTest => StorageTypeEnum.Mongo | StorageTypeEnum.Postgres;

  protected override IEnumerable<string> RequiredBaseStorageModules => new[]
  {
    nameof(IBasicStorageModule)
  };
  
  protected IBasicStorageModule GetBasicStorageModule(StorageTypeEnum storageType) => StorageResolver.FirstStorageModuleImplementation<IBasicStorageModule>(storageType);
}