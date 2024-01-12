using JMCore.Localizer;
using JMCore.Server.DB;
using JMCore.Server.Localizer;
using JMCore.Server.ResX;
using JMCore.Server.Services.JMCache;
using JMCore.Services.JMCache;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JMCore.Server;

public class ConfigureServerBuilder(IConfiguration configuration, IWebHostEnvironment environment)
{
    private Action<JMDbContextConfiguration>? _dbConfig;
    private bool _isDb;
    private bool _isLocalization;

    private IConfiguration Configuration { get; } = configuration;
    private IWebHostEnvironment Environment { get; } = environment;

    private List<ResXManagerInfo>? _addLanguageResources;

    public void ConfigureServerServices(IServiceCollection services)
    {
        if (_isDb)
            ConfigureDatabase(services);

        if (_isLocalization)
            ConfigureLocalization(services);
    }

    public void ConfigureServer(IApplicationBuilder app)
    {
        if (_isLocalization)
            app.UseRequestLocalization();

        if (!_isDb)
            return;

        using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var service = serviceScope.ServiceProvider;
        service.ConfigureJMDbAsync().Wait();

        //if (_isLocalization)
          //  service.UseJMServerLocalization().Wait();
    }

    public ConfigureServerBuilder SetDb(Action<JMDbContextConfiguration>? configureAction = null)
    {
        _dbConfig = configureAction;
        _isDb = true;
        return this;
    }

    public ConfigureServerBuilder SetLocalization(List<ResXManagerInfo>? addLanguageResources = null)
    {
        _addLanguageResources = addLanguageResources;
        _isLocalization = true;
        return this;
    }


    private void ConfigureDatabase(IServiceCollection services)
    {
        var connectionString = Configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentException("DefaultConnection is null.");

        services.AddJMMemoryCache<JMCacheServerCategory>();
        services.AddJMDb(connectionString, (o) => { _dbConfig?.Invoke(o); });
        if (Environment.IsDevelopment())
        {
            services.AddDatabaseDeveloperPageExceptionFilter();
        }
    }

    private void ConfigureLocalization(IServiceCollection services)
    {
        services.AddJMServerLocalization(option =>
        {
            if (_addLanguageResources != null)
                option.OtherResourceManager = _addLanguageResources;
            option.SupportedCultures = ResXRegister.SupportedCultures.Keys.ToList();
        });
        services.AddStringMemoryLocalization(options => { options.ReturnOnlyKeyIfNotFound = false; })
            .Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(ResXRegister.SupportedCultures.First().Value);
                options.AddSupportedCultures(ResXRegister.SupportedCultures.Values.ToArray());
                options.AddSupportedUICultures(ResXRegister.SupportedCultures.Values.ToArray());
            });
    }
}