namespace JMCore.Server.DB.Audit;

public class AuditDbConfiguration: IAuditDbConfiguration
{
    public IEnumerable<string> AuditEntities { get; init; } = new List<string>();
    public Dictionary<string, IEnumerable<string>> NotAuditProperty { get; init; } = new();
}