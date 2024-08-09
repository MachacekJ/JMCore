using ACore.Server.Services.SMS;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.Services.SMST;

public class SmsTestsBase : ServerTestsBase
{
    protected ISMSSenderJM SMSSender = null!;

    protected override async Task GetServices(IServiceProvider sp)
    {
        await base.GetServices(sp);
        SMSSender = sp.GetService<ISMSSenderJM>() ?? throw new ArgumentException($"{nameof(ISMSSenderJM)} is null.");
    }
}