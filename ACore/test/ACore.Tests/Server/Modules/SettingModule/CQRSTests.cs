using System.Reflection;
using ACore.Server.Modules.SettingModule.CQRS.SettingGet;
using ACore.Server.Modules.SettingModule.CQRS.SettingSave;
using Xunit;

namespace ACore.Tests.Server.Modules.SettingModule;

    public class CQRSTests : BasicStorageModuleEfContextTests
    {
        [Fact]
        public async Task SettingsCommandAndQueryTest()
        {
            const string key = "key";
            const string value = "value";

            var method = MethodBase.GetCurrentMethod();
            await RunTestAsync(method, async () =>
            {
                await Mediator.Send(new SettingSaveCommand(key, value));
                var result = await Mediator.Send(new SettingGetQuery(key));

                Assert.Equal(value, result);
            });
        }
    }

