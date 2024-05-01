using JMCore.Server.Storages.Audit;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Tests.ServerT.DbT.DbContexts.AuditStructureT;

public class AuditAttributeBaseT : AuditStructureBaseT
{
    protected override void RegisterServices(ServiceCollection sc)
    {
        base.RegisterServices(sc);
        var auditConfiguration = new AuditDbConfiguration();
        sc.AddSingleton<IAuditDbConfiguration>(auditConfiguration);
    }
}