namespace JMCore.Server.DB.Audit;

public interface IAuditUserProvider
{
    (string userId, string userName) GetUser();
}