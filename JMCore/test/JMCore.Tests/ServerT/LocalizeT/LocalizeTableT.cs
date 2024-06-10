using System.Globalization;
using System.Reflection;
using FluentAssertions;
using JMCore.Localizer;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace JMCore.Tests.ServerT.LocalizeT;

public class LocalizeTableT : LocalizeBaseT
{
    [Fact]
    public async Task CheckTranslations()
    {
        var method = MethodBase.GetCurrentMethod();
        await RunTestAsync(method, async () =>
        {
            CultureInfo.CurrentUICulture = new CultureInfo(1033);
            Assert.True(ResXTestClient["Test_2"] == "I would like to encourage you");
            CultureInfo.CurrentUICulture = new CultureInfo(1029);
            Assert.True(ResXTestServer["Test_1"] == "Ahoj světe");

            var ff = ResXTestServer["NotEx22"];
            Assert.True(ff.ResourceNotFound);
            Assert.True(ff.Value == "TestServer:NotEx22");


            var ff3 = ResXTestServer["Test_Param", "P1", "P2"];
            Assert.Contains("P1", ff3.Value);
            Assert.Contains("P2", ff3.Value);
            await Task.CompletedTask;
        });
    }

    /// <summary>
    /// https://www.codemag.com/Article/2009081/A-Deep-Dive-into-ASP.NET-Core-Localization
    /// </summary>
    [Fact]
    public async Task CheckDatabase()
    {
        var method = MethodBase.GetCurrentMethod();
        await RunTestAsync(method, async () =>
        {
            var allServer = await LocalizationMemoryEfStorageImpl.Localizations.Where(l => l.Scope.HasFlag(LocalizationScopeEnum.Server)).ToListAsync();
            Assert.True(allServer.Count() > 10);
            Assert.True(allServer.Count(a => a.MsgId == "TestClientF") == 0);
            Assert.True(allServer.Count(a => a.MsgId == "TestServerF" && a.Lcid == 1033) == 1);
            Assert.True(allServer.Count(a => a.MsgId == "TestBothF" && a.Lcid == 1033) == 1);

            var allClient = await LocalizationMemoryEfStorageImpl.Localizations.Where(l => l.Scope.HasFlag(LocalizationScopeEnum.Client)).ToListAsync();
            Assert.True(allClient.Count() > 10);
            Assert.True(allClient.All(a => a.MsgId != "TestServerF"));
            Assert.True(allClient.Count(a => a.MsgId == "TestClientF" && a.Lcid == 1033) == 1);
            Assert.True(allClient.Count(a => a.MsgId == "TestBothF" && a.Lcid == 1033) == 1);

            allServer.Single(c => c.MsgId == "Test_1" && c.Lcid == 1029 && c.ContextId == "TestServer").Translation.Should().Be("Ahoj světe");
            allClient.Single(c => c.MsgId == "Test_2" && c.Lcid == 1033 && c.ContextId == "TestClient").Translation.Should().Be("I would like to encourage you");
        });
    }
}