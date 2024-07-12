using JMCore.Server.Storages.Base.Audit.Configuration;
using JMCore.Server.Storages.Base.Audit.EF;
using JMCore.Server.Storages.Base.Audit.UserProvider;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Tests.ServerT.StoragesT.ModulesT.AuditStorageT;

public class AuditAttributeBaseT : AuditStorageBaseT
{
  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    sc.AddScoped<IAuditConfiguration, AuditConfiguration>();
    sc.AddScoped<IAuditDbService, AuditDbService>();
    sc.AddSingleton<IAuditUserProvider>(TestAuditUserProvider.CreateDefaultUser());
  }
}