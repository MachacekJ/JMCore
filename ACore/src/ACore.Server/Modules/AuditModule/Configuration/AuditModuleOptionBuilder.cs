using ACore.Server.Configuration.Modules;
using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Storages.Configuration;

namespace ACore.Server.Modules.AuditModule.Configuration;

public class AuditModuleOptionBuilder : ModuleOptionBuilder
{
  private IAuditConfiguration? _auditConfiguration;

  public static AuditModuleOptionBuilder Empty() => new();

  public AuditModuleOptionBuilder AuditConfiguration(IAuditConfiguration auditConfiguration)
  {
    _auditConfiguration = auditConfiguration;
    return this;
  }

  public AuditModuleOptionBuilder Storages(Action<StorageOptionBuilder> action)
  {
    StoragesBase(action);
    return this;
  }

  public AuditModuleOptions Build(StorageOptionBuilder? defaultStorages)
  {
    return new AuditModuleOptions(IsActive)
    {
      Storages = BuildStorage(defaultStorages, nameof(IAuditStorageModule)),
      ManualConfiguration = _auditConfiguration
    };
  }
}