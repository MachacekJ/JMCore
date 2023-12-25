using System.Collections;
using JMCore.Server.DB.DbContexts.LocalizeStructure;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentAssertions;
using JMCore.Localizer;
using JMCore.Server.DB.DbContexts.LocalizeStructure.Models;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace JMCore.Tests.ServerT.DbT.DbContexts.LocalizeStructureT;

public class LocalizeDbContextT : LocalizeStructureBaseT
{
    private ILocalizeDbContext _localizeDb = null!;
    private const string FakeContext = "fakeContext";

    protected override async Task GetServicesAsync(IServiceProvider sp)
    {
        await base.GetServicesAsync(sp);
        _localizeDb = sp.GetService<ILocalizeDbContext>() ?? throw new ArgumentException($"{nameof(Server.DB.DbContexts.LocalizeStructure.LocalizeDbContext)} is null.");
    }

    [Fact]
    public async Task AllSync()
    {
        const string fakeContext = "fakeContext";
        var method = MethodBase.GetCurrentMethod();
        await RunTestAsync(method, async () =>
        {
            // Arrange
            var newItems = new List<LocalizationEntity>
            {
                new()
                {
                    ContextId = fakeContext,
                    Lcid = 1033,
                    MsgId = "fake",
                    Scope = LocalizationScopeEnum.Client,
                    Translation = "fake translation"
                },
                new()
                {
                    ContextId = fakeContext,
                    Lcid = 1033,
                    MsgId = "fake2",
                    Scope = LocalizationScopeEnum.Client | LocalizationScopeEnum.Server,
                    Translation = "fake translation2"
                },
                new()
                {
                    ContextId = fakeContext,
                    Lcid = 1033,
                    MsgId = "fake3",
                    Scope = LocalizationScopeEnum.Server,
                    Translation = "fake translation3"
                }
            };

            newItems.ForEach(item => LocalizeDbContext.Localizations.Add(item));
            await LocalizeDbContext.SaveChangesAsync();

            // Act.
            var result = await _localizeDb.ClientLocalizations(1033, null);

            //Assert
            // One item is server scope
            result.Where(a => a.ContextId == fakeContext).Should().HaveCount(2);
        });
    }


    [Theory]
    [ClassData(typeof(LastSyncData))]
    public async Task LastSync(DateTime? firstDate, DateTime? secondDate, DateTime? thirdDate, DateTime? checkDate, int count)
    {
        var method = MethodBase.GetCurrentMethod();
        await RunTestAsync(method, async () =>
        {
            // Arrange
            var newItems = new List<LocalizationEntity>
            {
                new()
                {
                    ContextId = FakeContext,
                    Lcid = 1033,
                    MsgId = "fake",
                    Scope = LocalizationScopeEnum.Client,
                    Translation = "fake translation",
                    Changed = firstDate
                },
                new()
                {
                    ContextId = FakeContext,
                    Lcid = 1033,
                    MsgId = "fake2",
                    Scope = LocalizationScopeEnum.Client | LocalizationScopeEnum.Server,
                    Translation = "fake translation2",
                    Changed = secondDate
                },
                new()
                {
                    ContextId = FakeContext,
                    Lcid = 1033,
                    MsgId = "fake3",
                    Scope = LocalizationScopeEnum.Client,
                    Translation = "fake translation3",
                    Changed = thirdDate
                }
            };

            newItems.ForEach(item => LocalizeDbContext.Localizations.Add(item));
            await LocalizeDbContext.SaveChangesAsync();

            // Act.
            var result = await _localizeDb.ClientLocalizations(1033, checkDate);

            //Assert
            result.Where(a => a.ContextId == FakeContext).Should().HaveCount(count);
        });
    }

    [Fact]
    public async Task ChangeTranslation()
    {
        var newTans = "newTranslation";
        var method = MethodBase.GetCurrentMethod();
        await RunTestAsync(method, async () =>
        {
            // Arrange
            var allItems = await LocalizeDbContext.Localizations.Where(l => l.Scope == LocalizationScopeEnum.Client).ToListAsync();
            var item = allItems.First(a => a is { MsgId: "TestClientF", Lcid: 1033 });

            // Act
            await LocalizeDbContext.ChangeTranslationAsync(item.Id, newTans);

            // Assert
            ResXTestClient["TestClientF"].ToString().Should().Be(newTans);
            allItems = await LocalizeDbContext.Localizations.Where(l => l.Scope == LocalizationScopeEnum.Client).ToListAsync();
            allItems.First(a => a is { MsgId: "TestClientF", Lcid: 1033 }).Translation.Should().Be("newTranslation");
        });
    }


    private class LastSyncData : IEnumerable<object?[]>
    {
        private readonly DateTime _dateTime = new DateTime(2000, 10, 10, 10, 10, 10, DateTimeKind.Utc);

        public IEnumerator<object?[]> GetEnumerator()
        {
            yield return new object?[] { null, null, null, null, 3 };
            yield return new object?[] { _dateTime, _dateTime, _dateTime, null, 3 };
            yield return new object?[] { _dateTime, _dateTime, _dateTime, _dateTime, 0 };
            yield return new object?[] { _dateTime.AddSeconds(1), _dateTime.AddSeconds(-1), _dateTime, _dateTime, 1 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}