using ACore.Base.Modules;
using ACore.Server.Configuration.Modules;
using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Modules.AuditModule.UserProvider;
using ACore.Server.Storages.Configuration;

namespace ACore.Server.Modules.AuditModule.Configuration;

public class AuditModuleOptionBuilder : StorageModuleOptionBuilder, IModuleOptionsBuilder
{
  private IAuditUserProvider _auditUserProvider = new AuditEmptyUserProvider();
  private bool _isActive;
  public static AuditModuleOptionBuilder Empty() => new();
  
  public AuditModuleOptionBuilder UserProvider(IAuditUserProvider auditUserProvider)
  {
    _auditUserProvider = auditUserProvider;
    return this;
  }

  public AuditModuleOptionBuilder Storages(Action<StorageOptionBuilder> action)
  {
    StoragesBase(action);
    return this;
  }

  public AuditModuleOptions Build(StorageOptionBuilder? defaultStorages)
  {
    return new AuditModuleOptions
    {
      Storages = BuildStorage(defaultStorages, nameof(IAuditStorageModule)),
      AuditUserProvider = _auditUserProvider,
      IsActive = _isActive
    };
  }

  public void Activate()
  {
    _isActive = true;
  }
}