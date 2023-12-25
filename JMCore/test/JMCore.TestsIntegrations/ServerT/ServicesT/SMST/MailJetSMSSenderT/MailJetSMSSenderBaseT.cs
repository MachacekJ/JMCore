﻿using JMCore.Server.Services.Email.MailJet;
using JMCore.Server.Services.SMS;
using JMCore.Tests.ServerT.ServicesT.SMST;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace JMCore.TestsIntegrations.ServerT.ServicesT.SMST.MailJetSMSSenderT;

public class MailJetSMSSenderBaseT : SMSBaseT
{
    protected override void RegisterServices(ServiceCollection sc)
    {
        base.RegisterServices(sc);
        var mailJetEmailSender = MailJetOptions.CreateFromConfig(Configuration);
        sc.AddSingleton(Options.Create(mailJetEmailSender));
        sc.AddSingleton<ISMSSenderJM, MailJetSMSSender>();
    }
}