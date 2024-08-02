using ACore.Localizer.Storage;
using ACore.Server.Modules.LocalizationModule.Storage;
using ACore.Server.ResX;
using ACore.Server.ResX.Helpers;
using ACore.Server.Storages;
using ACore.Server.Storages.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ACore.Server.Modules.LocalizationModule.Configuration;

public static class LocalizerExtensions
{
  public static void AddJMServerLocalization(this IServiceCollection service, Action<ResXLocalizationOptions> options)
  {
    service.Configure(options);
  }

  public static async Task UseJMServerLocalization(this IServiceProvider service)
  {
    var storageResolver = service.GetService<IStorageResolver>()
                          ?? throw new ArgumentException($"Service for {nameof(ILocalizationStorageModule)} not found.");

    var localizeStorageModule = storageResolver.FirstReadWriteStorage<ILocalizationStorageModule>()
                                ?? throw new ArgumentException($"Service for {nameof(ILocalizationStorageModule)} not found.");

    var localizationProvider = service.GetService<ILocalizationStorage>()
                               ?? throw new ArgumentException($"Service for {nameof(ILocalizationStorage)} not found.");

    var options = service.GetService<IOptions<ResXLocalizationOptions>>()
                  ?? throw new ArgumentException($"Service for {nameof(ResXLocalizationOptions)} not found.");
    
    await LoadLocalizationStorageAsync(options, localizeStorageModule, localizationProvider);
  }

  /// <summary>
  /// Combine resX as default and EF as source of translations.
  /// </summary>
  public static async Task LoadLocalizationStorageAsync(IOptions<ResXLocalizationOptions> options, ILocalizationStorageModule db, ILocalizationStorage localizationProvider)
  {
    var rexOptions = options.Value;

    var dic = ResXSourceRegister.DefaultSourceResx();
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