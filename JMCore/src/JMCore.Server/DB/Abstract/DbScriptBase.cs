namespace JMCore.Server.DB.Abstract;

public abstract class DbScriptBase
{
    public abstract IEnumerable<DbVersionScriptsBase> AllVersions { get; }
}

