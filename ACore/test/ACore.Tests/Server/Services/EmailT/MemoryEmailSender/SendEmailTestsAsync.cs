using System.Reflection;
using Xunit;

namespace ACore.Tests.Server.Services.EmailT.MemoryEmailSender;

public class SendEmailTestsAsync : MemoryEmailTestsSenderBase
{
    [Fact]
    public async Task BaseTest()
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