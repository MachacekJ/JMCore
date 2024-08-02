using ACore.Server.Services.Email;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.Services.EmailT;

public class EmailBaseTests : ServerBaseTests
{
    protected IEmailSenderJM EmailSenderJM = null!;

    protected override async Task GetServicesAsync(IServiceProvider sp)
    {
        await base.GetServicesAsync(sp);

        EmailSenderJM = sp.GetRequiredService<IEmailSenderJM>();
        if (EmailSenderJM == null)
            throw new Exception("EmailBaseT IEmailSenderJM is not implemented.");
    }
}