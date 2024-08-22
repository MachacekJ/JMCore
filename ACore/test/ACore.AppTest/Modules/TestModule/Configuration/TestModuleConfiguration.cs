using ACore.Server.Configuration;
using ACore.Server.Modules.AuditModule.Configuration;

namespace ACore.AppTest.Modules.TestModule.Configuration;

public class TestModuleConfiguration : StorageModuleConfiguration
{
  public AuditConfiguration? AuditManualConfig { get; set; }
}
