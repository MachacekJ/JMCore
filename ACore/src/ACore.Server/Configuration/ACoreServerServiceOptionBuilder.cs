using ACore.Configuration;
using ACore.Server.Modules.SettingModule.Configuration;
using ACore.Server.Storages.Configuration;

namespace ACore.Server.Configuration;

public class ACoreServerServiceOptionBuilder
{
  private readonly ACoreServiceOptionBuilder _aCoreServiceOptionBuilder = ACoreServiceOptionBuilder.Empty();
  private readonly SettingModuleOptionBuilder _settingModuleOptionBuilder = SettingModuleOptionBuilder.Empty();
  private StorageOptionBuilder? _storageOptionBuilder;
  private ACoreServerServiceOptionBuilder()
  {
  }

  public static ACoreServerServiceOptionBuilder Empty() => new();

  public ACoreServerServiceOptionBuilder DefaultStorage(Action<StorageOptionBuilder> action)
  {
    _storageOptionBuilder ??= StorageOptionBuilder.Empty();
    action(_storageOptionBuilder);
    return this;
  }

  public ACoreServerServiceOptionBuilder AddSettingModule(Action<SettingModuleOptionBuilder> action)
  {
    action(_settingModuleOptionBuilder);
    return this;
  }

  public ACoreServerServiceOptionBuilder ACore(Action<ACoreServiceOptionBuilder> action)
  {
    action(_aCoreServiceOptionBuilder);
    return this;
  }

  public ACoreServerServiceOptions Build() => new()
  {
    DefaultStorages = _storageOptionBuilder?.Build(),
    ACoreServiceOptions = _aCoreServiceOptionBuilder.Build(),
    SettingModuleOptions = _settingModuleOptionBuilder.Build()
  };
}