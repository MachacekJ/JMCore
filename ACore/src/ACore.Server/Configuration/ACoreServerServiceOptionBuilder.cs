using ACore.Configuration;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.SettingModule.Configuration;
using ACore.Server.Storages.Configuration;

namespace ACore.Server.Configuration;

public class ACoreServerServiceOptionBuilder
{
  private readonly ACoreServiceOptionBuilder _aCoreServiceOptionBuilder = ACoreServiceOptionBuilder.Empty();
  private readonly SettingModuleOptionBuilder _settingModuleOptionBuilder = SettingModuleOptionBuilder.Empty();
  private  readonly AuditModuleOptionBuilder _auditModuleOptionBuilder = AuditModuleOptionBuilder.Empty();
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

  public ACoreServerServiceOptionBuilder AddSettingModule(Action<SettingModuleOptionBuilder>? action = null)
  {
    action?.Invoke(_settingModuleOptionBuilder);
    _settingModuleOptionBuilder.Activate();
    return this;
  }

  public ACoreServerServiceOptionBuilder AddAuditModule(Action<AuditModuleOptionBuilder> action)
  {
    action(_auditModuleOptionBuilder);
    _auditModuleOptionBuilder.Activate();
    return this;
  }

  public ACoreServerServiceOptionBuilder ACore(Action<ACoreServiceOptionBuilder> action)
  {
    action(_aCoreServiceOptionBuilder);
    return this;
  }

  public ACoreServerServiceOptions Build()
  {
    return new ACoreServerServiceOptions
    {
      DefaultStorages = _storageOptionBuilder?.Build(),
      ACoreServiceOptions = _aCoreServiceOptionBuilder.Build(),
      SettingModuleOptions = _settingModuleOptionBuilder.Build(_storageOptionBuilder),
      AuditModuleOptions = _auditModuleOptionBuilder.Build(_storageOptionBuilder)
    };
  }
}