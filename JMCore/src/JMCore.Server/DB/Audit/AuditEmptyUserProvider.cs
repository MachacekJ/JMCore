namespace JMCore.Server.DB.Audit;

public class AuditEmptyUserProvider : IAuditUserProvider
{
    public (string userId, string userName) GetUser()
    {
        return (string.Empty, "Unknown");
    }
}

