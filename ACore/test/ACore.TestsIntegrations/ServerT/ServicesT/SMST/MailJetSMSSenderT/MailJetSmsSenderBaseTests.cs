using ACore.Server.Services.Email.MailJet;
using ACore.Server.Services.SMS;
using ACore.Tests.Server.Services.SMST;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ACore.TestsIntegrations.ServerT.ServicesT.SMST.MailJetSMSSenderT;

public class MailJetSmsSenderBaseTests : SmsBaseTests
{
    protected override void RegisterServices(ServiceCollection sc)
    {
        base.RegisterServices(sc);
        var mailJetEmailSender = MailJetOptions.CreateFromConfig(Configuration);
        sc.AddSingleton(Options.Create(mailJetEmailSender));
        sc.AddSingleton<ISMSSenderJM, MailJetSMSSender>();
    }
}