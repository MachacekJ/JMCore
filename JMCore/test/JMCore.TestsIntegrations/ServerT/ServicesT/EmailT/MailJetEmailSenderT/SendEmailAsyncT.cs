using System.Reflection;
using JMCore.Tests.TestModelsT;
using Xunit;

namespace JMCore.TestsIntegrations.ServerT.ServicesT.EmailT.MailJetEmailSenderT;

public class SendEmailAsyncT : MailJetEmailSenderBaseT
{
    [Fact]
    public async Task MailMachacekJM()
    {
        var method = MethodBase.GetCurrentMethod();
        await RunTestAsync(new TestData(method)
        {
            TestEnvironmentType = TestEnvironmentTypeEnum.Dev,
        }, async () =>
        {
            await EmailSenderJM.SendEmailAsync("machacekjm@seznam.cz", "test", "testmsg");
        });
    }
}