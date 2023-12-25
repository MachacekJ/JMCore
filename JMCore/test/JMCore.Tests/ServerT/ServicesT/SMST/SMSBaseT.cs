using JMCore.Server.Services.SMS;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Tests.ServerT.ServicesT.SMST;

public class SMSBaseT : ServerTestBaseT
{
    protected ISMSSenderJM SMSSender = null!;

    protected override async Task GetServicesAsync(IServiceProvider sp)
    {
        await base.GetServicesAsync(sp);
        SMSSender = sp.GetService<ISMSSenderJM>() ?? throw new ArgumentException($"{nameof(ISMSSenderJM)} is null.");
    }
}