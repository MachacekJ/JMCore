using ACore.Base.CQRS.Results;
using ACore.Server.Storages.Definitions.Models;

namespace ACore.Server.Modules.SettingsDbModule.CQRS.SettingsDbGet;

public class SettingsDbGetQuery(StorageTypeEnum storageType, string key, bool isRequired = false) : SettingsDbModuleRequest<Result<string?>>
{
  public StorageTypeEnum StorageType { get; } = storageType;

  public string Key { get; } = key;
  public bool IsRequired { get; } = isRequired;

  public SettingsDbGetQuery(string key, bool isRequired = false) : this(StorageTypeEnum.All, key, isRequired)
  {
  }
}