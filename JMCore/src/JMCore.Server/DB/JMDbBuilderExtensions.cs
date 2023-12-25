using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using JMCore.Server.DB.Abstract;
using JMCore.Server.DB.Audit;
using JMCore.Server.DB.DbContexts.AuditStructure;
using JMCore.Server.DB.DbContexts.BasicStructure;
using JMCore.Server.DB.DbContexts.LocalizeStructure;
using Microsoft.Extensions.Options;

namespace JMCore.Server.DB;

public static class JMDbBuilderExtensions
{
    public static void AddJMDb(this IServiceCollection services, string connectionString,
        Action<JMDbContextConfiguration>? configureAction = null)
    {

        services.AddMediatR((c) =>
        {
            c.RegisterServicesFromAssemblyContaining(typeof(IBasicDbContext));
        });

        var option = new JMDbContextConfiguration(services, connectionString)
        {
            AuditStructure = false,
            LanguageStructure = false
        };
        option.AddJMDbContext<IBasicDbContext, BasicDbContext>();

        configureAction?.Invoke(option);
        services.AddSingleton<IJMDbContextConfiguration>(option);

        if (option.AuditStructure)
        {
            option.AddJMDbContext<IAuditDbContext, AuditDbContext>();

            services.TryAddScoped<IAuditDbService, AuditDbService>();
            services.TryAddSingleton<IAuditUserProvider, AuditEmptyUserProvider>();

            var auditConfiguration = option.AuditDbConfiguration;
            services.TryAddSingleton(auditConfiguration);
        }

        if (option.LanguageStructure)
        {
            option.AddJMDbContext<ILocalizeDbContext, LocalizeDbContext>();
        }

        services.Add(option.AllDbContext);
    }

    public static async Task<IServiceProvider> ConfigureJMDbAsync(this IServiceProvider serviceProvider)
    {
        var option = serviceProvider.GetService<IJMDbContextConfiguration>();
        if (option == null)
            throw new Exception("JMCoreDBOptions is null.");

        await serviceProvider.GetService<IBasicDbContext>()?.UpdateDbAsync()!;

        if (option.AuditStructure)
        {
            var auditService = serviceProvider.GetService<IAuditDbService>();
            if (auditService == null)
                throw new ArgumentNullException($"Missing {nameof(IAuditDbService)} service.");

            var auditBbContext = serviceProvider.GetService<IAuditDbContext>();
            if (auditBbContext == null)
                throw new ArgumentNullException($"Missing {nameof(IAuditDbContext)} service.");

            var auditBbOption = serviceProvider.GetService<IOptions<AuditDbConfiguration>>();
            if (auditBbOption == null)
                throw new ArgumentNullException($"Missing {nameof(IOptions<AuditDbConfiguration>)} service.");

            await serviceProvider.GetService<IAuditDbContext>()?.UpdateDbAsync()!;
        }
        
        foreach (var s in option.AllDbContext)
        {
            var se = serviceProvider.GetService(s.ServiceType);
            if (se is not IDbContextBase ii)
                continue;

            switch (se)
            {
                case IAuditDbContext:
                case IBasicDbContext:
                    continue;
                default:
                    await ii.UpdateDbAsync();
                    break;
            }
        }

        return serviceProvider;
    }
}