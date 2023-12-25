using JMCore.Server.Services.Email;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Tests.ServerT.ServicesT.EmailT.MemoryEmailSenderT
{
    public class MemoryEmailSenderBaseT : EmailBaseT
    {
        protected override void RegisterServices(ServiceCollection sc)
        {
            base.RegisterServices(sc);
            sc.AddSingleton<IEmailSenderJM, MemoryEmailSender>();
        }

    }
}
