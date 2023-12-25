using JMCore.Server.Services.SMS;
using System.Reflection;
using Xunit;

namespace JMCore.Tests.ServerT.ServicesT.SMST.MemorySMSSenderT
{
    public class SendSMSAsyncT : MemorySMSSenderBaseT
    {
        [Fact]
        public async Task Ok()
        {
            var method = MethodBase.GetCurrentMethod();
            await RunTestAsync(method, async () =>
            {
                await SMSSender.SendSMSAsync("trenden", "+420777329682", "Test");

                var mm = SMSSender as MemorySMSSender;
                Assert.True(mm!.AllSMS.Count == 1);
            });
        }
    }
}
