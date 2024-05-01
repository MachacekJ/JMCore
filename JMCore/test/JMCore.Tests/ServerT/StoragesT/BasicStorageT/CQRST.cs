using System.Reflection;
using JMCore.Server.CQRS.Storages.BasicModule.SettingGet;
using JMCore.Server.CQRS.Storages.BasicModule.SettingSave;
using Xunit;

namespace JMCore.Tests.ServerT.StoragesT.BasicStorageT;

    public class CQRST : BasicStorageModuleEfContextT
    {
        [Fact]
        public async Task SettingsCommandAndQuery()
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

