using ACore.Server.Storages.Models;

namespace ACore.TestsIntegrations.Modules.TestModule.PG;

public class PGAuditTestBase : AuditStructureBaseTests
{
  protected StorageTypeEnum? StorageTypesToTest => StorageTypeEnum.Postgres;
}