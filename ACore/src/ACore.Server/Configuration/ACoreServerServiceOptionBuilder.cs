using ACore.Configuration;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.SettingsDbModule.Configuration;
using ACore.Server.Storages.Configuration;

namespace ACore.Server.Configuration;

public class ACoreServerServiceOptionBuilder
{
  private readonly ACoreOptionsBuilder _aCoreOptionsBuilder = ACoreOptionsBuilder.Empty();
  private readonly SettingsDbModuleOptionsBuilder _settingsDbModuleOptionsBuilder = SettingsDbModuleOptionsBuilder.Empty();
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

  public ACoreServerServiceOptionBuilder AddSettingModule(Action<SettingsDbModuleOptionsBuilder>? action = null)
  {
    action?.Invoke(_settingsDbModuleOptionsBuilder);
    _settingsDbModuleOptionsBuilder.Activate();
    return this;
  }

  public ACoreServerServiceOptionBuilder AddAuditModule(Action<AuditServerModuleOptionBuilder> action)
  {
    action(_auditServerModuleOptionBuilder);
    _auditServerModuleOptionBuilder.Activate();
    return this;
  }

  public ACoreServerServiceOptionBuilder ACore(Action<ACoreOptionsBuilder>? action = null)
  {
    action?.Invoke(_aCoreOptionsBuilder);
    return this;
  }

  public ACoreServerServiceOptions Build()
  {
    return new ACoreServerServiceOptions
    {
      DefaultStorages = _storageOptionBuilder?.Build(),
      ACoreOptions = _aCoreOptionsBuilder.Build(),
      SettingsDbModuleOptions = _settingsDbModuleOptionsBuilder.Build(_storageOptionBuilder),
      AuditServerModuleOptions = _auditServerModuleOptionBuilder.Build(_storageOptionBuilder)
    };
  }
}