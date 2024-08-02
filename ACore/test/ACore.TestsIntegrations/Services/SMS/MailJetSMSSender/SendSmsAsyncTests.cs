using System.Reflection;
using ACore.Tests.BaseInfrastructure.Models;
using Xunit;

namespace ACore.TestsIntegrations.Services.SMS.MailJetSMSSender
{
    public class SendSmsAsyncTests : MailJetSmsSenderBaseTests
    {

        [Fact]
        public async Task Ok()
        {
            var method = MethodBase.GetCurrentMethod();
            await RunTestAsync(new TestData(method)
            {
              //  TestEnvironmentType = TestEnvironmentTypeEnum.Dev,
            }, async () =>
            {
                await SMSSender.SendSMSAsync("trenden", "+420777329682", "Integration Test");
            });
        }

    }


}
