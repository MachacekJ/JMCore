using ACore.Server.Services.Email;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.Tests.Server.Services.EmailT.MemoryEmailSender
{
    public class MemoryEmailTestsSenderBase : EmailTestsBase
    {
        protected override void RegisterServices(ServiceCollection sc)
        {
            base.RegisterServices(sc);
            sc.AddSingleton<IEmailSenderJM, ACore.Server.Services.Email.MemoryEmailSender>();
        }

    }
}
