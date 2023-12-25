using System.Reflection;
using JMCore.Tests.TestModelsT;
using Xunit;

namespace JMCore.TestsIntegrations.ServerT.ServicesT.SMST.MailJetSMSSenderT
{
    public class SendSMSAsyncT : MailJetSMSSenderBaseT
    {

        [Fact]
        public async Task Ok()
        {
            var method = MethodBase.GetCurrentMethod();
            await RunTestAsync(new TestData(method)
            {
                TestEnvironmentType = TestEnvironmentTypeEnum.Dev,
            }, async () =>
            {
                await SMSSender.SendSMSAsync("trenden", "+420777329682", "Integration Test");
            });
        }

    }


}
