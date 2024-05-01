using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Storages.Base.EF;

namespace JMCore.Server.CQRS.DB.BasicStructure.SettingGet;

public class SettingGetQuery : IDbRequest<string?>
{
  public SettingGetQuery(StorageTypeEnum storageType, string key, bool isRequired = false)
  {
    Key = key;
    StorageType = storageType;
    IsRequired = isRequired;
  }
    
  public StorageTypeEnum StorageType { get; }

  public string Key { get; }
  public bool IsRequired { get; }
}