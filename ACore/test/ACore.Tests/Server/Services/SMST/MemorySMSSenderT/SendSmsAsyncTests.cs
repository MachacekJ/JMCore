using System.Reflection;
using ACore.Server.Services.SMS;
using Xunit;

namespace ACore.Tests.Server.Services.SMST.MemorySMSSenderT
{
    public class SendSmsAsyncTests : MemorySmsSenderBaseTests
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
