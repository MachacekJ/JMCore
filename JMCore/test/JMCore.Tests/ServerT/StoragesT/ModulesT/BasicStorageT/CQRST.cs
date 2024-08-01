using System.Reflection;
using JMCore.Server.Modules.SettingModule.CQRS.SettingGet;
using JMCore.Server.Modules.SettingModule.CQRS.SettingSave;
using Xunit;

namespace JMCore.Tests.ServerT.StoragesT.ModulesT.BasicStorageT;

    public class CQRST : BasicStorageModuleEfContextT
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

