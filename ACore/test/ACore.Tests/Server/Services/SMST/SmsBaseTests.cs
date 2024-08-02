using ACore.Server.Services.SMS;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.Services.SMST;

public class SmsBaseTests : ServerBaseTests
{
    protected ISMSSenderJM SMSSender = null!;

    protected override async Task GetServicesAsync(IServiceProvider sp)
    {
        await base.GetServicesAsync(sp);
        SMSSender = sp.GetService<ISMSSenderJM>() ?? throw new ArgumentException($"{nameof(ISMSSenderJM)} is null.");
    }
}