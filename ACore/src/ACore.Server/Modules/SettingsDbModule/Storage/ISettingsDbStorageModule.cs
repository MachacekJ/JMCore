using ACore.Server.Storages;

namespace ACore.Server.Modules.SettingsDbModule.Storage;

public interface ISettingsDbStorageModule : IStorage
{
  Task<string?> Setting_GetAsync(string key, bool isRequired = true);
  Task Setting_SaveAsync(string key, string value, bool isSystem = false);
}