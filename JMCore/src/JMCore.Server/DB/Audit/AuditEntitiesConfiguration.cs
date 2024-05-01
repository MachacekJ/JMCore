namespace JMCore.Server.DB.Audit;

public class AuditEntitiesConfiguration: IAuditEntitiesConfiguration
{
    public IEnumerable<string> AuditEntities { get; init; } = new List<string>();
    public Dictionary<string, IEnumerable<string>> NotAuditProperty { get; init; } = new();
}