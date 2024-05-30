using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.Storages.Base.EF;

namespace JMCore.Server.CQRS.Storages.BasicModule.SettingGet;

public class SettingGetQuery(StorageTypeEnum storageType, string key, bool isRequired = false) : IDbRequest<string?>
{
  public StorageTypeEnum StorageType { get; } = storageType;

  public string Key { get; } = key;
  public bool IsRequired { get; } = isRequired;

  public SettingGetQuery(string key, bool isRequired = false) : this(StorageTypeEnum.AllRegistered, key, isRequired)
  {
  }
}