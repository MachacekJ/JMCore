using JMCore.Server.Services.Email;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Tests.Server.Services.EmailT.MemoryEmailSender
{
    public class MemoryEmailSenderBaseTests : EmailBaseTests
    {
        protected override void RegisterServices(ServiceCollection sc)
        {
            base.RegisterServices(sc);
            sc.AddSingleton<IEmailSenderJM, JMCore.Server.Services.Email.MemoryEmailSender>();
        }

    }
}
