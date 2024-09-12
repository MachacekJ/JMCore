using ACore.Base;
using ACore.Server.Configuration.Modules;
using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Storages.Configuration;

namespace ACore.Server.Modules.AuditModule.Configuration;

public class AuditServerModuleOptionBuilder : ServerModuleOptionBuilder, IModuleOptionsBuilder
{
  private IAuditConfiguration? _auditConfiguration;
  private bool _isActive;
  public static AuditServerModuleOptionBuilder Empty() => new();

  public AuditServerModuleOptionBuilder ManualAuditConfiguration(IAuditConfiguration auditConfiguration)
  {
    _auditConfiguration = auditConfiguration;
    return this;
  }

  public AuditServerModuleOptionBuilder Storages(Action<StorageOptionBuilder> action)
  {
    StoragesBase(action);
    return this;
  }

  public AuditServerModuleOptions Build(StorageOptionBuilder? defaultStorages)
  {
    return new AuditServerModuleOptions
    {
      Storages = BuildStorage(defaultStorages, nameof(IAuditStorageModule)),
      ManualConfiguration = _auditConfiguration,
      IsActive = _isActive
    };
  }

  public void Activate()
  {
    _isActive = true;
  }
}