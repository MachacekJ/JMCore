namespace JMCore.Server.Storages.Audit;

public interface IAuditUserProvider
{
    (string userId, string userName) GetUser();
}