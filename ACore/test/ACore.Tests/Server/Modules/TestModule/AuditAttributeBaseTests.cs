using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.AuditModule.EF;
using ACore.Server.Modules.AuditModule.UserProvider;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.Modules.TestModule;

public class AuditAttributeBaseTests : AuditStorageBaseTests
{
  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    sc.AddScoped<IAuditConfiguration, AuditConfiguration>();
    sc.AddScoped<IAuditDbService, AuditDbService>();
    sc.AddSingleton<IAuditUserProvider>(TestAuditUserProvider.CreateDefaultUser());
  }
}