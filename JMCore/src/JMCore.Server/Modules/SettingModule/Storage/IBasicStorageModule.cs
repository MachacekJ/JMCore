namespace JMCore.Server.Modules.SettingModule.Storage;

public interface IBasicStorageModule
{
  Task<string?> Setting_GetAsync(string key, bool isRequired = true);
  Task Setting_SaveAsync(string key, string value, bool isSystem = false);
}