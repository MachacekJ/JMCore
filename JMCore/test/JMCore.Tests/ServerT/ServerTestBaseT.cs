namespace JMCore.Tests.ServerT;

public abstract class ServerTestBaseT : TestBaseT
{
    protected static string GetDbName()
    {
        return Guid.NewGuid().ToString();
    }
}