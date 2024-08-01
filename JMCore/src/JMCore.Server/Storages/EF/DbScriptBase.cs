namespace JMCore.Server.Storages.EF;

public abstract class DbScriptBase
{
    public abstract IEnumerable<DbVersionScriptsBase> AllVersions { get; }
}

