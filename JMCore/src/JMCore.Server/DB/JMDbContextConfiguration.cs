using JMCore.Server.DB.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Server.DB;

public class JMDbContextConfiguration : IJMDbContextConfiguration
{
    private string ConnectionString { get; set; }
    
    public ServiceCollection AllDbContext { get; set; } = new();
    public bool LanguageStructure { get; set; } = true;
    public bool AuditStructure { get; set; } = true;
    public IAuditDbConfiguration AuditDbConfiguration { get; set; } = new AuditDbConfiguration();


    private readonly IServiceCollection _serviceCollection;
    public JMDbContextConfiguration(IServiceCollection services, string cn)
    {
        ConnectionString = cn;
        _serviceCollection = services;
    }

    public void AddJMDbContext<TInterface, TDbContext>()
       where TDbContext : DbContext, TInterface
       where TInterface : class
    {
        _serviceCollection.AddDbContext<TDbContext>(opt => opt.UseSqlServer(ConnectionString));
        AddDbScoped<TInterface, TDbContext>();
    }

    private void AddDbScoped<TInterface, TDbContext>()
        where TDbContext : class, TInterface
        where TInterface : class
    {
        AllDbContext.AddScoped<TInterface, TDbContext>();
    }
}
