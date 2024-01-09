using System.Globalization;
using System.Net.Http.Headers;
using JMCore.Blazor;
using JMCore.Blazor.Localization;
using JMCore.Blazor.Services.App;
using JMCore.Blazor.Services.Page;
using JMCore.Blazor.Services.Page.Implementations;
using JMCore.Client.Services.Http;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Telerik.Blazor.Services;

namespace JMCoreTest.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();
            builder.Services.TryAddScoped<AuthenticationStateProvider,HostAuthenticationStateProvider>();
          //  builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<HostAuthenticationStateProvider>());
           // builder.Services.AddTransient<AuthorizedHandler>();
            
            var con = new ConfigureClientBuilder().SetLocalization();
            con.ConfigureServices(builder.Services, builder.Configuration);
#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            
            var appConfig = new AppStartConfiguration(false);
            builder.Services.AddSingleton<IAppStartConfiguration>(appConfig);
            builder.Services.AddSingleton<IAppState, AppState>();
            builder.Services.AddSingleton<IPageManager, PageManager>();
            builder.Services.AddJMBlazor();
// register a custom localizer for the Telerik components, after registering the Telerik services
            builder.Services.AddSingleton<ITelerikStringLocalizer, JMTelerikStringLocalizer>();
            
            var res = builder.Build();
            con.Configure(res).Wait();

            return res;
        }
    }
}
