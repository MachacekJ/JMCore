using JMCore.Server.Storages;

namespace JMCore.Server.Modules.SettingModule.Storage;

public interface IBasicStorageModule : IStorage
{
  Task<string?> Setting_GetAsync(string key, bool isRequired = true);
  Task Setting_SaveAsync(string key, string value, bool isSystem = false);
}