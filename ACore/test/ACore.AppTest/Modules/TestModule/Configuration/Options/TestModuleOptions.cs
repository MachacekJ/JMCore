using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Storages.Configuration.Options;

namespace ACore.AppTest.Modules.TestModule.Configuration.Options;

public class TestModuleOptions : ACoreStorageOptions
{
  public AuditConfiguration? AuditManualConfig { get; set; }
}
