using System.Globalization;
using System.Resources;
using JMCore.Localizer;
using JMCore.Server.Localizer;
using JMCore.Server.ResX;
using JMCore.Server.Storages;
using JMCore.Server.Storages.DbContexts.LocalizeStructure;
using JMCore.Services.JMCache;
using JMCore.Tests.ServerT.LocalizeT.ResX;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace JMCore.Tests.ServerT.LocalizeT;

public class LocalizeBaseT : ServerTestBaseT
{
    protected LocalizeDbContext LocalizeDbContext = null!;
    protected IStringLocalizer<TestServer> ResXTestServer = null!;
    protected IStringLocalizer<TestClient> ResXTestClient = null!;
    protected IStringLocalizer<JMCore.ResX.ResX_Errors> ResXCoreErrors = null!;

    protected override void RegisterServices(ServiceCollection sc)
    {
        base.RegisterServices(sc);
        sc.AddDbContext<LocalizeDbContext>(opt => opt.UseInMemoryDatabase(GetDbName()));
        sc.AddScoped<ILocalizeDbContext, LocalizeDbContext>();

        var option = new JMDbContextConfiguration(sc, string.Empty)
        {
            LanguageStructure = true
        };
        sc.AddSingleton<IJMDbContextConfiguration>(option);
        sc.AddJMMemoryCache<JMCacheCategory>();
        RegisterLanguageResources(sc,
            new List<ResXManagerInfo>
            {
                new(
                    nameof(TestServer),
                    new ResourceManager(typeof(TestServer)),
                    LocalizationScopeEnum.Server),
                new(
                    nameof(TestClient),
                    new ResourceManager(typeof(TestClient)),
                    LocalizationScopeEnum.Client),
                new(
                    nameof(TestBoth),
                    new ResourceManager(typeof(TestBoth)),
                    LocalizationScopeEnum.Client | LocalizationScopeEnum.Server)
            });
    }

    protected override async Task GetServicesAsync(IServiceProvider sp)
    {
        await base.GetServicesAsync(sp);
        await LocalizeResourcesAsync(sp);

        LocalizeDbContext = sp.GetService<LocalizeDbContext>() ?? throw new ArgumentException($"{nameof(LocalizeDbContext)} is null.");
        ResXTestServer = sp.GetService<IStringLocalizer<TestServer>>() ?? throw new ArgumentException($"{nameof(IStringLocalizer<TestServer>)} is null.");
        ResXTestClient = sp.GetService<IStringLocalizer<TestClient>>() ?? throw new ArgumentException($"{nameof(IStringLocalizer<TestClient>)} is null.");
        ResXCoreErrors = sp.GetService<IStringLocalizer<JMCore.ResX.ResX_Errors>>() ?? throw new ArgumentException($"{nameof(IStringLocalizer<JMCore.ResX.ResX_Errors>)} is null.");
    }

    private void RegisterLanguageResources(ServiceCollection sc,
        List<ResXManagerInfo> addLanguageResources)
    {
        sc.AddJMServerLocalization(option =>
        {
            option.OtherResourceManager = addLanguageResources;
            option.SupportedCultures = ResXRegister.SupportedCultures.Keys.ToList();
        });

        sc.AddStringMemoryLocalization(options => { options.ReturnOnlyKeyIfNotFound = false; })
            .Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.AddSupportedCultures(ResXRegister.SupportedCultures.Values.ToArray());
                options.AddSupportedUICultures(ResXRegister.SupportedCultures.Values.ToArray());
            });

        CultureInfo.CurrentUICulture = new CultureInfo(1033);
        CultureInfo.CurrentCulture = new CultureInfo(1033);
    }

    private async Task LocalizeResourcesAsync(IServiceProvider sp)
    {
        await sp.UseJMServerLocalization();
    }
}