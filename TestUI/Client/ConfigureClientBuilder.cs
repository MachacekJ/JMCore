using System.Collections.Specialized;
using System.Globalization;
using JMCore.Blazor.CQRS.LocalStorage;
using JMCore.Blazor.CQRS.LocalStorage.Models;
using JMCore.Blazor.Services;
using JMCore.Blazor.Services.App;
using JMCore.Client.CQRS.Http;
using JMCore.Client.Services.Http;
using JMCore.Localizer;
using JMCore.Localizer.Storage;
using JMCore.Models.BaseRR;
using JMCoreTest.Blazor.Shared.Controllers.LocalizationController;
using MediatR;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;

namespace JMCoreTest.Blazor.Client;

public class ConfigureClientBuilder
{
    private bool _isLocalization;

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        if (_isLocalization)
        {
            services.AddStringMemoryLocalization(options => { options.ReturnOnlyKeyIfNotFound = false; });
        }
    }

    public async Task Configure(WebAssemblyHost host)
    {
        var httpClientFactory = host.Services.GetService<IJMHttpClientFactory>();
        if (httpClientFactory == null)
            throw new ArgumentException($"{nameof(IJMHttpClientFactory)} is not registered.");

        var mediator = host.Services.GetService<IMediator>();
        if (mediator == null)
            throw new ArgumentException($"{nameof(IMediator)} is not registered.");

        var locService = host.Services.GetService<ILocalizationStorage>();
        if (locService == null)
            throw new ArgumentException($"{nameof(ILocalizationStorage)} is not registered.");


        var js = host.Services.GetService<IJSRuntime>();
        var culture = new CultureInfo("en-US");
        if (js != null)
        {
            var cookieCulture = await js.GetAspNetCoreCultureCookie();

            if (!string.IsNullOrWhiteSpace(cookieCulture))
                culture = new CultureInfo(cookieCulture);
            else
                await js.SetAspNetCoreCultureCookie("en-US");


            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }

        var key = $"Localization-{culture.LCID}";
        var local = await mediator.Send(new LocalStorageGetQuery(LocalStorageCategoryEnum.Resources, key));
        TranslationCacheValue transVal;
        if (!local.IsValue)
        {
            var query = new NameValueCollection
            {
                ["lcid"] = culture.LCID.ToString(),
                //   ["lastSyncDateTime"] = DateTime.MinValue.ToUniversalTime().ToString("O")
            };

            var res = await mediator.Send(new HttpNonAuthorizedCommand<ClientLanguagePackAsyncResponse>(ApiMethod.Get, "api/Localization/ClientLanguagePack", new ApiRequestBase() { LCID = culture.LCID }, typeof(ApiRequestBase)));

            var bb = res.Data.Where(l => l.Changed != null).MaxBy(l => l.Changed);
            var lastSync = DateTime.MaxValue;
            if (bb != null)
                lastSync = bb.Changed.Value;

            transVal = new TranslationCacheValue()
            {
                LastSync = lastSync,
                Records = res.Data.ToList()
            };

            await mediator.Send(new LocalStorageSaveCommand(LocalStorageCategoryEnum.Resources, key, transVal, transVal.GetType()));
        }
        else
        {
            transVal = local.GetValue<TranslationCacheValue>()!;
        }

        locService.PopulateLocalizationStorage(transVal.Records);
        host.Services.GetRequiredService<IAppStartConfiguration>().ApplyTranslations(locService);
    }

    public ConfigureClientBuilder SetLocalization()
    {
        _isLocalization = true;
        return this;
    }

    public class TranslationCacheValue
    {
        public DateTime LastSync { get; set; }
        public List<LocalizationRecord> Records { get; set; }
    }
}