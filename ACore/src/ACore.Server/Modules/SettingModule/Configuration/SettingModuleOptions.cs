using ACore.Server.Storages.Configuration.Options;

namespace ACore.Server.Modules.SettingModule.Configuration;

public class SettingModuleOptions
{
  public ACoreStorageOptions Storages { get; init; } = new();
}