using JMCore.Localizer;
using JMCore.Server.Localizer;
using JMCore.Server.Storages;
using JMCore.Server.Storages.DbContexts.LocalizeStructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.TestsIntegrations.ServerT.DbT.LocalizeStructureT;

public class LocalizeStructureBaseT : DbBaseT
{
    protected LocalizeDbContext Db = null!;

    protected override void RegisterServices(ServiceCollection sc)
    {
        var supportedCulture =  Server.ResX.ResXRegister.SupportedCultures;
        base.RegisterServices(sc);
        sc.AddJMDb(ConnectionString, o =>
        {
            o.LanguageStructure = true;
        });
        sc.AddJMServerLocalization(option =>
        {
            option.SupportedCultures = supportedCulture.Keys.ToList();
        });
        sc.AddStringMemoryLocalization(options => { options.ReturnOnlyKeyIfNotFound = false; })
            .Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.AddSupportedCultures(supportedCulture.Values.ToArray());
                options.AddSupportedUICultures(supportedCulture.Values.ToArray());
            });
    }

    protected override async Task GetServicesAsync(IServiceProvider sp)
    {
        await base.GetServicesAsync(sp);
        await sp.ConfigureJMDbAsync();
        await sp.UseJMServerLocalization();
        Db = sp.GetService<LocalizeDbContext>() ?? throw new ArgumentException($"{nameof(LocalizeDbContext)} is null.");
    }
}