using ACore.Base.CQRS.Models;
using ACore.Server.Storages.Models;

namespace ACore.Server.Modules.SettingsDbModule.CQRS.SettingsGet;

public class SettingsesDbDbGetQuery(StorageTypeEnum storageType, string key, bool isRequired = false) : SettingsDbModuleRequest<Result<string?>>
{
  public StorageTypeEnum StorageType { get; } = storageType;

  public string Key { get; } = key;
  public bool IsRequired { get; } = isRequired;

  public SettingsesDbDbGetQuery(string key, bool isRequired = false) : this(StorageTypeEnum.AllRegistered, key, isRequired)
  {
  }
}