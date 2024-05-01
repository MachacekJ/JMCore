using JMCore.Server.Storages.Base.Audit.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Tests.ServerT.StoragesT.AuditStorageT;

public class AuditAttributeBaseT : AuditStorageBaseT
{
  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    sc.AddScoped<IAuditConfiguration, AuditConfiguration>();
  }
}