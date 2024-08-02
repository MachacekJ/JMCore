using System.Reflection;
using Xunit;

namespace JMCore.Tests.Server.Services.EmailT.MemoryEmailSender;

public class SendEmailAsyncTests : MemoryEmailSenderBaseTests
{
    [Fact]
    public async Task Ok()
    {
        var method = MethodBase.GetCurrentMethod();
        await RunTestAsync(method, async () =>
        {
            await EmailSenderJM.SendEmailAsync("machacekjm", "test", "test");

            var mm = EmailSenderJM as JMCore.Server.Services.Email.MemoryEmailSender;
            Assert.True(mm!.AllEmails.Count == 1);

        });
    }

}