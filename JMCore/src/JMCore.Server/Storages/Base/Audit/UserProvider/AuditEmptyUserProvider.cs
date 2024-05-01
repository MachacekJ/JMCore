namespace JMCore.Server.Storages.Base.Audit.UserProvider;

public class AuditEmptyUserProvider : IAuditUserProvider
{
    public (string userId, string userName) GetUser()
    {
        return (string.Empty, "Unknown");
    }
}

