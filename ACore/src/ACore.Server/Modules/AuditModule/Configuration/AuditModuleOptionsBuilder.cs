using ACore.Server.Configuration.Modules;
using ACore.Server.Modules.AuditModule.Storage;
using ACore.Server.Modules.AuditModule.UserProvider;
using ACore.Server.Storages.Configuration;

namespace ACore.Server.Modules.AuditModule.Configuration;

public class AuditModuleOptionsBuilder : StorageModuleOptionBuilder
{
  private IAuditUserProvider _auditUserProvider = new AuditEmptyUserProvider();
  public static AuditModuleOptionsBuilder Empty() => new();

  public AuditModuleOptionsBuilder UserProvider(IAuditUserProvider auditUserProvider)
  {
    _auditUserProvider = auditUserProvider;
    return this;
  }

  public AuditModuleOptionsBuilder Storages(Action<StorageOptionBuilder> action)
  {
    StoragesBase(action);
    return this;
  }

  public AuditModuleOptions Build(StorageOptionBuilder? defaultStorages)
  {
    return new AuditModuleOptions(IsActive)
    {
      Storages = BuildStorage(defaultStorages, nameof(IAuditStorageModule)),
      AuditUserProvider = _auditUserProvider,
    };
  }
}