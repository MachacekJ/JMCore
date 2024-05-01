using JMCore.Server.Storages.Base.Audit.UserProvider;

namespace JMCore.Tests.ServerT.StoragesT.AuditStorageT;

public enum TestAuditUserTypeEnum
{
    Default,
    Admin
}

public class TestAuditUserProvider : IAuditUserProvider
{
    private int _userId;
    private string _userName = "";


    public static TestAuditUserProvider CreateDefaultUser()
    {
        return new TestAuditUserProvider(TestAuditUserTypeEnum.Default);
    }

    public void SetContext(TestAuditUserTypeEnum userType)
    {
        _userId = userType switch
        {
            TestAuditUserTypeEnum.Default => 0,
            TestAuditUserTypeEnum.Admin => 1,
            _ => _userId
        };
        _userName = $"user{_userId}";
    }

    private TestAuditUserProvider(TestAuditUserTypeEnum userType)
    {
        SetContext(userType);
    }

    public (string userId, string userName) GetUser()
    {
        return ($"{_userId}", _userName);
    }
}