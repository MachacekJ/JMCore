using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Server.Configuration.DB;

public interface IJMDbContextConfiguration
{
    bool LanguageStructure { get; set; }
    bool AuditStructure { get; set; }
    ServiceCollection AllDbContext { get; set; }
}