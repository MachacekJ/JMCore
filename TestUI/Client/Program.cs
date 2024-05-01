using System.Globalization;
using System.Net;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net.Http.Headers;
using Autofac.Extensions.DependencyInjection;
using JMCore.Blazor.Services.App;
using JMCore.Client.Services.Http;
using JMCoreTest.Blazor.Client;
using JMCoreTest.Blazor.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Autofac;
using JMCore.Blazor;
using JMCore.Client.CQRS.Http;
using JMCore.Blazor.Services.Page.Implementations;
using JMCore.Blazor.Services.Page.Interfaces;
using JMCore.Client.Services.Logger;
using JMCoreTest.Blazor.Client.Localization;
using Telerik.Blazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.ConfigureContainer(new AutofacServiceProviderFactory(ConfigureContainer));

builder.Logging.SetMinimumLevel(LogLevel.Information);
builder.Logging.AddProvider(new InMemoryLoggingProvider());

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.TryAddSingleton<AuthenticationStateProvider, HostAuthenticationStateProvider>();
builder.Services.TryAddSingleton(sp => (HostAuthenticationStateProvider)sp.GetRequiredService<AuthenticationStateProvider>());
builder.Services.AddTransient<AuthorizedHandler>();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var con = new ConfigureClientBuilder().SetLocalization();
con.ConfigureServices(builder.Services, builder.Configuration);

#region appConfig
var isForE2ETests = false;

#if TEST
var http = new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
};

using var response = await http.GetAsync("app.json");
if (response.StatusCode == HttpStatusCode.OK)
{
    await using var stream = await response.Content.ReadAsStreamAsync();
    builder.Configuration.AddJsonStream(stream);
    if (builder.Configuration["E2E:Enabled"] == "true")
        isForE2ETests = true;
}
#endif

var appConfig = new AppStartConfiguration(isForE2ETests);
builder.Services.AddSingleton<IAppStartConfiguration>(appConfig);
builder.Services.AddSingleton<IAppState, AppState>();
builder.Services.AddSingleton<IPageManager, PageManager>();
builder.Services.AddJMBlazor();
// register a custom localizer for the Telerik components, after registering the Telerik services
builder.Services.AddSingleton<ITelerikStringLocalizer, JMTelerikStringLocalizer>();
#endregion

builder.Services.AddTransient<IJMHttpClientFactory, AntiforgeryHttpClientFactory>();
builder.Services.AddHttpClient(AntiforgeryHttpClientFactory.NonAuthorizedClientName, client =>
{
    client.DefaultRequestHeaders.AcceptLanguage.Clear();
    client.DefaultRequestHeaders.AcceptLanguage.ParseAdd(CultureInfo.DefaultThreadCurrentCulture?.TwoLetterISOLanguageName);
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});
builder.Services.AddHttpClient(AntiforgeryHttpClientFactory.AuthorizedClientName, client =>
{
    client.DefaultRequestHeaders.AcceptLanguage.Clear();
    client.DefaultRequestHeaders.AcceptLanguage.ParseAdd(CultureInfo.DefaultThreadCurrentCulture?.TwoLetterISOLanguageName);
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
}).AddHttpMessageHandler<AuthorizedHandler>();

var res = builder.Build();
await con.Configure(res);
await res.RunAsync();
return;


static void ConfigureContainer(ContainerBuilder containerBuilder)
{
    containerBuilder.RegisterGeneric(typeof(HttpAuthorizedHandler<>)).AsImplementedInterfaces();
    containerBuilder.RegisterGeneric(typeof(HttpNonAuthorizedHandler<>)).AsImplementedInterfaces();
}