namespace JMCore.Server.Storages.Abstract;

public abstract class DbScriptBase
{
    public abstract IEnumerable<DbVersionScriptsBase> AllVersions { get; }
}

