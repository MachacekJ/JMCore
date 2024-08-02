using System.Reflection;
using ACore.Tests.BaseInfrastructure.Models;
using Xunit;

namespace ACore.TestsIntegrations.ServerT.ServicesT.EmailT.MailJetEmailSenderT;

public class SendEmailAsyncTests : MailJetEmailSenderBaseTests
{
    [Fact]
    public async Task MailMachacekJM()
    {
        var method = MethodBase.GetCurrentMethod();
        await RunTestAsync(new TestData(method)
        {
            //TestEnvironmentType = TestEnvironmentTypeEnum.Dev,
        }, async () =>
        {
            await EmailSenderJM.SendEmailAsync("machacekjm@seznam.cz", "test", "testmsg");
        });
    }
}