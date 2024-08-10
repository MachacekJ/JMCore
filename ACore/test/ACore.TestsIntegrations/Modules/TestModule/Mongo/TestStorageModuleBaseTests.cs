using ACore.Server.Storages.Models;
using ACore.TestsIntegrations.Modules.TestModule.PG;

namespace ACore.TestsIntegrations.Modules.TestModule.Mongo;

public class MongoAuditTestBase : AuditStructureBaseTests
{
  protected StorageTypeEnum? StorageTypesToTest => StorageTypeEnum.Mongo;
}