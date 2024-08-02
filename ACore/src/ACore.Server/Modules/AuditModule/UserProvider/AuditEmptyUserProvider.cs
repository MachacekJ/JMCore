namespace ACore.Server.Modules.AuditModule.UserProvider;

public class AuditEmptyUserProvider : IAuditUserProvider
{
    public (string userId, string userName) GetUser()
    {
        return (string.Empty, "Unknown");
    }
}

