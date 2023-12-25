﻿using JMCore.Server.Services.Email;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Tests.ServerT.ServicesT.EmailT;

public class EmailBaseT : ServerTestBaseT
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