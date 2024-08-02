using System.Globalization;
using System.Resources;
using ACore.Localizer;
using ACore.ResX;
using ACore.Server.MemoryStorage;
using ACore.Server.MemoryStorage.LocalizationModule;
using ACore.Server.Modules.LocalizationModule.Storage;
using ACore.Server.Modules.SettingModule.Storage;
using ACore.Server.ResX;
using ACore.Server.Services.JMCache;
using ACore.Tests.Server.Modules.LocalizationModule.LocalizeT.ResX;
using ACore.Tests.Server.Storages;
using ACore.Modules.CacheModule;
using ACore.Server.Modules.LocalizationModule.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace ACore.Tests.Server.Modules.LocalizationModule.LocalizeT;

public class LocalizeBaseTests : StorageBaseTests
{
  protected ILocalizationStorageModule LocalizationStorageModule = null!;
  protected IStringLocalizer<TestServer> ResXTestServer = null!;
  protected IStringLocalizer<TestClient> ResXTestClient = null!;
  protected IStringLocalizer<ResX_Errors> ResXCoreErrors = null!;
  protected LocalizationMemoryEfStorageImpl LocalizationMemoryEfStorageImpl = null!;

  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    StorageResolver.RegisterStorage(sc, new MemoryStorageConfiguration(new[] { nameof(IBasicStorageModule), nameof(ILocalizationStorageModule) }));
    sc.AddJMMemoryCache<JMCacheServerCategory>();
    var addLanguageResources = new List<ResXSource>
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
    };

    sc.AddJMServerLocalization(option =>
    {
      option.OtherResourceManager = addLanguageResources;
      option.SupportedCultures = ResXSourceRegister.SupportedCultures.Keys.ToList();
    });

    sc.AddStringMemoryLocalization(options => { options.ReturnOnlyKeyIfNotFound = false; })
      .Configure<RequestLocalizationOptions>(options =>
      {
        options.DefaultRequestCulture = new RequestCulture("en-US");
        options.AddSupportedCultures(ResXSourceRegister.SupportedCultures.Values.ToArray());
        options.AddSupportedUICultures(ResXSourceRegister.SupportedCultures.Values.ToArray());
      });

    CultureInfo.CurrentUICulture = new CultureInfo(1033);
    CultureInfo.CurrentCulture = new CultureInfo(1033);
  }

  protected override async Task GetServicesAsync(IServiceProvider sp)
  {
    await base.GetServicesAsync(sp);
    await sp.UseJMServerLocalization();

    LocalizationStorageModule = StorageResolver.FirstReadWriteStorage<ILocalizationStorageModule>(); //sp.GetService<LocalizeStorageModule>() ?? throw new ArgumentException($"{nameof(LocalizeStorageModule)} is null.");
    LocalizationMemoryEfStorageImpl = (LocalizationStorageModule as LocalizationMemoryEfStorageImpl) ?? throw new ArgumentException();
    ResXTestServer = sp.GetService<IStringLocalizer<TestServer>>() ?? throw new ArgumentException($"{nameof(IStringLocalizer<TestServer>)} is null.");
    ResXTestClient = sp.GetService<IStringLocalizer<TestClient>>() ?? throw new ArgumentException($"{nameof(IStringLocalizer<TestClient>)} is null.");
    ResXCoreErrors = sp.GetService<IStringLocalizer<ResX_Errors>>() ?? throw new ArgumentException($"{nameof(IStringLocalizer<ResX_Errors>)} is null.");
  }
}