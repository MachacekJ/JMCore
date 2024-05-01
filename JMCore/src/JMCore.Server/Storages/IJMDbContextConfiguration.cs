using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Server.Storages;

public interface IJMDbContextConfiguration
{
    bool LanguageStructure { get; set; }
    bool AuditStructure { get; set; }
    ServiceCollection AllDbContext { get; set; }
}