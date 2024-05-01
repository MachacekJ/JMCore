namespace JMCore.Server.Storages.Audit;

public class AuditEmptyUserProvider : IAuditUserProvider
{
    public (string userId, string userName) GetUser()
    {
        return (string.Empty, "Unknown");
    }
}

