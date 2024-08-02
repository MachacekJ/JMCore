namespace ACore.Server.Modules.AuditModule.Configuration;

public class AuditConfiguration: IAuditConfiguration
{
    public IEnumerable<string> AuditEntities { get; init; } = new List<string>();
    public Dictionary<string, IEnumerable<string>> NotAuditProperty { get; init; } = new();
}