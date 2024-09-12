using ACore.Configuration;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.SettingModule.Configuration;
using ACore.Server.Storages.Configuration;

namespace ACore.Server.Configuration;

public class ACoreServerServiceOptionBuilder
{
  private readonly ACoreOptionBuilder _aCoreOptionBuilder = ACoreOptionBuilder.Empty();
  private readonly SettingServerModuleOptionBuilder _settingServerModuleOptionBuilder = SettingServerModuleOptionBuilder.Empty();
  private  readonly AuditServerModuleOptionBuilder _auditServerModuleOptionBuilder = AuditServerModuleOptionBuilder.Empty();
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

  public ACoreServerServiceOptionBuilder AddSettingModule(Action<SettingServerModuleOptionBuilder>? action = null)
  {
    action?.Invoke(_settingServerModuleOptionBuilder);
    _settingServerModuleOptionBuilder.Activate();
    return this;
  }

  public ACoreServerServiceOptionBuilder AddAuditModule(Action<AuditServerModuleOptionBuilder> action)
  {
    action(_auditServerModuleOptionBuilder);
    _auditServerModuleOptionBuilder.Activate();
    return this;
  }

  public ACoreServerServiceOptionBuilder ACore(Action<ACoreOptionBuilder>? action = null)
  {
    action?.Invoke(_aCoreOptionBuilder);
    return this;
  }

  public ACoreServerServiceOptions Build()
  {
    return new ACoreServerServiceOptions
    {
      DefaultStorages = _storageOptionBuilder?.Build(),
      ACoreOptions = _aCoreOptionBuilder.Build(),
      SettingServerModuleOptions = _settingServerModuleOptionBuilder.Build(_storageOptionBuilder),
      AuditServerModuleOptions = _auditServerModuleOptionBuilder.Build(_storageOptionBuilder)
    };
  }
}