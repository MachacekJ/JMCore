using ACore.Server.Services.Email;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.Services.EmailT;

public class EmailBase : ServerBase
{
    protected IEmailSenderJM EmailSenderJM = null!;

    protected override async Task GetServices(IServiceProvider sp)
    {
        await base.GetServices(sp);

        EmailSenderJM = sp.GetRequiredService<IEmailSenderJM>();
        if (EmailSenderJM == null)
            throw new Exception("EmailBaseT IEmailSenderJM is not implemented.");
    }
}