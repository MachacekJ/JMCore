using System.Reflection;
using Xunit;

namespace ACore.Tests.Server.Services.EmailT.MemoryEmailSender;

public class SendEmailAsync : MemoryEmailSenderBase
{
    [Fact]
    public async Task Ok()
    {
        var method = MethodBase.GetCurrentMethod();
        await RunTestAsync(method, async () =>
        {
            await EmailSenderJM.SendEmailAsync("machacekjm", "test", "test");

            var mm = EmailSenderJM as ACore.Server.Services.Email.MemoryEmailSender;
            Assert.True(mm!.AllEmails.Count == 1);

        });
    }

}