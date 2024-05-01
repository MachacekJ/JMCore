using JMCore.Server.DB;
using JMCore.Server.DB.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Server.DataStorages;

public class JMDbContextConfiguration: IJMDbContextConfiguration
{
  public Dictionary<string, List<Type>> AllContexts => new();
  public bool LanguageStructure { get; set; }
  public bool AuditStructure { get; set; }
  public ServiceCollection AllDbContext { get; set; }
}