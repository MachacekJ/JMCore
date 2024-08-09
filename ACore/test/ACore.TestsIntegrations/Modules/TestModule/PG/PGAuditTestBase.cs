using ACore.Server.Storages.Models;

namespace ACore.TestsIntegrations.Modules.TestModule.PG;

public class PGAuditBase : AuditStructureBase
{
  protected StorageTypeEnum StorageTypesToTest => StorageTypeEnum.Postgres;
}