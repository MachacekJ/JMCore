using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace JMCore.TestsIntegrations.ServerT.DbT.BasicStructureT;

public class UpdateDbT : BasicStructureBaseT
{
    [Fact]
    public async Task Ok()
    {
        var method = MethodBase.GetCurrentMethod();
        await RunTestAsync(method, async () =>
        {
            var res = await PGDb.Settings.CountAsync();
            Assert.True(res > 0);
        });
    }
}