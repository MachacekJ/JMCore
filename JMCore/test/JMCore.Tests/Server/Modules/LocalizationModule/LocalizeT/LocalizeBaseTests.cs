using System.Globalization;
using System.Resources;
using JMCore.Localizer;
using JMCore.Modules.CacheModule;
using JMCore.Server.MemoryStorage;
using JMCore.Server.MemoryStorage.LocalizationModule;
using JMCore.Server.Modules.LocalizationModule.Configuration;
using JMCore.Server.Modules.LocalizationModule.Storage;
using JMCore.Server.Modules.SettingModule.Storage;
using JMCore.Server.ResX;
using JMCore.Server.Services.JMCache;
using JMCore.Tests.Server.Modules.LocalizationModule.LocalizeT.ResX;
using JMCore.Tests.Server.Storages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace JMCore.Tests.Server.Modules.LocalizationModule.LocalizeT;

public class LocalizeBaseTests : StorageBaseTests
{
  protected ILocalizationStorageModule LocalizationStorageModule = null!;
  protected IStringLocalizer<TestServer> ResXTestServer = null!;
  protected IStringLocalizer<TestClient> ResXTestClient = null!;
  protected IStringLocalizer<JMCore.ResX.ResX_Errors> ResXCoreErrors = null!;
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
    ResXCoreErrors = sp.GetService<IStringLocalizer<JMCore.ResX.ResX_Errors>>() ?? throw new ArgumentException($"{nameof(IStringLocalizer<JMCore.ResX.ResX_Errors>)} is null.");
  }
}