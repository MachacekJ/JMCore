namespace JMCore.Server.Storages.Base.Audit.UserProvider;

public interface IAuditUserProvider
{
    (string userId, string userName) GetUser();
}