using ACore.Server.Services.Email;
using ACore.Server.Services.Email.MailJet;
using ACore.Tests.Server.Services.EmailT;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ACore.TestsIntegrations.ServerT.ServicesT.EmailT.MailJetEmailSenderT
{
    public class MailJetEmailSenderBaseTests : EmailBaseTests
    {
        protected override void RegisterServices(ServiceCollection sc)
        {
            base.RegisterServices(sc);
            var mailJetEmailSender = MailJetOptions.CreateFromConfig(Configuration);
            sc.AddSingleton(Options.Create(mailJetEmailSender));
            sc.AddSingleton<IEmailSenderJM, MailJetEmailSender>();
        }
    }
}
