using System.Reflection;
using JMCore.Server.Configuration.Storage.Models;
using JMCore.Server.CQRS.DB.BasicStructure.SettingGet;
using JMCore.Server.CQRS.DB.BasicStructure.SettingSave;
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
                await Mediator.Send(new SettingSaveCommand(StorageTypeEnum.Memory, key, value));
                var result = await Mediator.Send(new SettingGetQuery(StorageTypeEnum.Memory, key));

                Assert.Equal(value, result);
            });
        }
    }

