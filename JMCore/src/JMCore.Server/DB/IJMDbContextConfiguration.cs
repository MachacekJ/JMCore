using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Server.DB;

public interface IJMDbContextConfiguration
{
    bool LanguageStructure { get; set; }
    bool AuditStructure { get; set; }
    ServiceCollection AllDbContext { get; set; }
}