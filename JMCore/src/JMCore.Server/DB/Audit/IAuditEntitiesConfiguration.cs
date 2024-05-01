namespace JMCore.Server.DB.Audit;

public interface IAuditEntitiesConfiguration
{
    /// <summary>
    /// Primary use <see cref="AuditableAttribute"/>
    /// Using for e.g. IdentityDb
    /// </summary>
    public IEnumerable<string> AuditEntities { get; }

    /// <summary>
    /// Primary use <see cref="AuditableAttribute"/> and <see cref="NotAuditableAttribute"/>
    /// Using for e.g. IdentityDb
    /// </summary>
    Dictionary<string, IEnumerable<string>> NotAuditProperty { get; }
}

