using ACore.Server.Storages.Models;

namespace ACore.TestsIntegrations.Modules.TestModule.Mongo;

public class MongoAuditBase : AuditStructureBase
{
  protected static StorageTypeEnum StorageTypesToTest => StorageTypeEnum.Mongo;
}