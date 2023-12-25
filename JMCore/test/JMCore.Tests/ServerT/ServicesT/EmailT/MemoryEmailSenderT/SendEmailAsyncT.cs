using JMCore.Server.Services.Email;
using System.Reflection;
using Xunit;

namespace JMCore.Tests.ServerT.ServicesT.EmailT.MemoryEmailSenderT;

public class SendEmailAsyncT : MemoryEmailSenderBaseT
{
    [Fact]
    public async Task Ok()
    {
        var method = MethodBase.GetCurrentMethod();
        await RunTestAsync(method, async () =>
        {
            await EmailSenderJM.SendEmailAsync("machacekjm", "test", "test");

            var mm = EmailSenderJM as MemoryEmailSender;
            Assert.True(mm!.AllEmails.Count == 1);

        });
    }

}