using ACore.Server.Services.Email;
using JMCoreTest.Blazor.Server.Configuration;
using JMCoreTest.Blazor.Server.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(serverOptions => { serverOptions.AddServerHeader = false; });

var services = builder.Services;
var configuration = builder.Configuration;
var env = builder.Environment;

configuration.AddJsonFile("appsettings.Test.json", true, true);
services.Configure<E2EConfiguration>(configuration.GetSection("E2E"));

// var configServerBuilder = new ConfigureServerBuilder(configuration, env)
//     .SetDb(o =>
//     {
//         o.LanguageStructure = true;
//         o.AuditStructure = true;
//     })
//     .SetLocalization(ResXInfrastructure.RegisterResX());

//configServerBuilder.ConfigureServerServices(services);


services.AddScoped<IEmailSenderJM, MemoryEmailSender>();

services.AddAntiforgery(options =>
{
    options.HeaderName = "X-XSRF-TOKEN";
    options.Cookie.Name = "__Host-X-XSRF-TOKEN";
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

services.AddHttpClient();
services.AddOptions();

services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddOpenIdConnect(options =>
    {
        configuration.GetSection("OpenIDConnectSettings").Bind(options);
        options.Authority = configuration["OpenIDConnectSettings:Authority"];
        options.ClientId = configuration["OpenIDConnectSettings:ClientId"];
        options.ClientSecret = configuration["OpenIDConnectSettings:ClientSecret"];

        options.CallbackPath = "/signin-oidc";
        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.ResponseType = OpenIdConnectResponseType.Code;

        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = "name",
            RoleClaimType = "role"
        };
    });

services.AddControllersWithViews(options =>
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute())
);

services.AddRazorPages().AddMvcOptions(options =>
{
    //var policy = new AuthorizationPolicyBuilder()
    //    .RequireAuthenticatedUser()
    //    .Build();
    //options.Filters.Add(new AuthorizeFilter(policy));
});

var app = builder.Build();
//configServerBuilder.ConfigureServer(app);
if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
}

//app.UseSecurityHeaders(
//   SecurityHeadersDefinitions.GetHeaderPolicyCollection(env.IsDevelopment(),
//       configuration["OpenIDConnectSettings:Authority"]!));

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseNoUnauthorizedRedirect("/api");

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapNotFound("/api/{**segment}");
app.MapFallbackToPage("/_Host");

app.Run();