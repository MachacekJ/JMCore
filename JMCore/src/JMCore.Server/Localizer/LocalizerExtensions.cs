using JMCore.Localizer.Storage;
using JMCore.Server.ResX;
using JMCore.Server.Storages.DbContexts.LocalizeStructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace JMCore.Server.Localizer;

public static class LocalizerExtensions
{
    public static void AddJMServerLocalization(this IServiceCollection service, Action<ResXLocalizationOptions> options)
    {
        service.Configure(options);
    }

    public static async Task UseJMServerLocalization(this IServiceProvider service)
    {
        var db = service.GetService<ILocalizeDbContext>();
        if (db == null)
            throw new ArgumentException($"Service for {nameof(ILocalizeDbContext)} not found.");
        
        var localizationProvider = service.GetService<ILocalizationStorage>();
        if (localizationProvider == null)
            throw new ArgumentException($"Service for {nameof(ILocalizationStorage)} not found.");

        var options = service.GetService<IOptions<ResXLocalizationOptions>>();
        if (options == null)
            throw new ArgumentException($"Service for {nameof(ResXLocalizationOptions)} not found.");

        await LoadLocalizationStorageAsync(options, db, localizationProvider);
    }

    /// <summary>
    /// Combine resX as default and EF as source of translations.
    /// </summary>
    public static async Task LoadLocalizationStorageAsync(IOptions<ResXLocalizationOptions> options, ILocalizeDbContext db, ILocalizationStorage localizationProvider)
    {
        var rexOptions = options.Value;

        var dic = ResXRegister.DefaultCoreResx();
        dic.AddRange(rexOptions.OtherResourceManager);

        var allResources = ResXLoader.LoadFromResx(dic, rexOptions.SupportedCultures);
        foreach (var res in allResources)
        {
            // Translation can be changed manually in DB and default resX translation must be replaced.
            var trans = await db.SyncResXItemAsync(res);
            res.SetTranslation(trans);
        }

        localizationProvider.PopulateLocalizationStorage(allResources);
    }
}