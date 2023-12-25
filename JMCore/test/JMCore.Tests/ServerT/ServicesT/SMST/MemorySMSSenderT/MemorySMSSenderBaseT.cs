using JMCore.Server.Services.SMS;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Tests.ServerT.ServicesT.SMST.MemorySMSSenderT;

public class MemorySMSSenderBaseT : SMSBaseT
{
    protected override void RegisterServices(ServiceCollection sc)
    {
        base.RegisterServices(sc);
        sc.AddSingleton<ISMSSenderJM, MemorySMSSender>();
    }
}